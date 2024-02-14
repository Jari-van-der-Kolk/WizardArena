using UnityEngine.AI;
using Saxon.Sensor;
using Saxon.BT.AI.Controller;
using UnityEngine;
using Saxon.HashGrid;
using Saxon.NodePositioning;
using Saxon.BT.AI;
using System.Collections.Generic;
using System;
using UnityEngine.Experimental.AI;

namespace Saxon.BT
{
   

    public abstract class Agent
    {
        public abstract AgentTypes agentType { get; protected set; }
        public AgentController agentController;
        public NavMeshAgent navMesh;
        public ObjectDetection detection;
        public Vector3 lastKnownTargetPosition;

        public Agent(AgentController agentController)
        {
            this.agentController = agentController;
            this.navMesh = agentController.navMesh;
            this.detection = agentController.objectDetection;
        }

        public CommandInvoker commandInvoker => agentController.commandInvoker;
        public Transform target => detection.target;
        public Transform transform => agentController.transform;
        public Vector3 position => agentController.transform.position;
        public float reachedLocationDistance = 3f;
        public bool isAgentAtDestination => navMesh.remainingDistance <= navMesh.stoppingDistance;
        public bool hasTargetInSight => detection.hasTargetInSight;
        public bool hasTargetOcclusion => detection.HasOcclusionWithTarget();
        public SpatialHashGrid<HashNode> spatialHashGrid => NodeGenerator.Instance.hashGrid;
        public abstract BehaviourTree CreateTree();
        public void SetDestination(Vector3 destination) => agentController.SetDestination(destination);


        public List<T> SearchComponentsInArea<T>(List<T> targetList,float radius) where T : Component
        {
            List<T> result = new List<T>();
            for (int i = 0; i < targetList.Count; i++)
            {
                targetList[i].TryGetComponent(out T component);
                if (component != null)
                {
                    result.Add(component);
                }
            }

            return result;
        }

        public void Print(object message)
        {
            Debug.Log(message);
        }

        #region Nodes
        
        public bool IsWithinDistanceCheck(float distance)
        {
            return navMesh.remainingDistance <= distance;
        }

        //1
        public Node TargetInSight()
        {
            return new ConditionNode(() => hasTargetInSight);
        }

        //2
        public Node TargetOutOfSight()
        {
            return new ConditionNode(() => detection.noVisualsOnTarget);
        }
        //3
        public Node HasOcclusion()
        {
            return new ConditionNode(() => hasTargetOcclusion);
        }

        public Node HasNoOcclusion()
        {
            return new ConditionNode(() => !hasTargetOcclusion);
        }

        //4
        public Node InRangeOfTarget(float range)
        {
            return new ConditionNode(() => Saxon.IsInDistance(position, target, range));
        }
        
        //5
        public Node RecentlyLostTarget()
        {
            return new ConditionNode(() => detection.targetRecentlyLost);
        }

        //6
        public Node FoundTarget()
        {
            return new SequenceNode(new List<Node>
            {
                TargetInSight(), InRangeOfTarget(detection.data.closeRangeAttackDistance)
            });
        }

        //7
        public Node NoSightOnPlayerStatus()
        {
            return new ConditionNode(() => detection.lostTarget || detection.targetRecentlyLost);
        }

        public Node RotateTowardsTarget()
        {
            RotateTowardsCommand rotateTowardsTargetCommand = new RotateTowardsCommand(transform, this);
            ExecuteCommandNode rotateTowardsTarget = new ExecuteCommandNode(this, rotateTowardsTargetCommand, false);

            return new SequenceNode(new List<Node>
            {
                HasNoOcclusion(), RecentlyLostTarget(), InRangeOfTarget(detection.data.longRangeAttackDistance),rotateTowardsTarget
            });
        }

        public Node ChasePlayer(float reachedTargetDistance)
        {
            MoveToDestinationCommand standStillCommand = new MoveToDestinationCommand(navMesh, detection);
            ExecuteCommandNode standStill = new ExecuteCommandNode(this, standStillCommand);

            RotateTowardsCommand rotateTowardsTargetCommand = new RotateTowardsCommand(transform, this);
            ExecuteCommandNode rotateTowardsTarget = new ExecuteCommandNode(this, rotateTowardsTargetCommand, false);

            MoveToDestinationCommand moveTowardsTargetCommand = new MoveToDestinationCommand(navMesh, detection);
            MoveToDestinationCommand goToTowardsTargetCommand = new MoveToDestinationCommand(navMesh, detection);
            ExecuteCommandNode moveTowardsTarget = new ExecuteCommandNode(this, moveTowardsTargetCommand);
            ExecuteCommandNode goToTarget = new ExecuteCommandNode(this, goToTowardsTargetCommand);


            SequenceNode moveToTarget = new SequenceNode("move", new List<Node>
            {
                RecentlyLostTarget(), moveTowardsTarget        
            });


            SequenceNode rotate = new SequenceNode("rotate",new List<Node>
            {
                HasNoOcclusion(), RecentlyLostTarget(), InRangeOfTarget(detection.data.longRangeAttackDistance), rotateTowardsTarget
            });


            SequenceNode engage = new SequenceNode(new List<Node>
            {
                HasOcclusion(), goToTarget
            });


            ReturnStateNode pause = new ReturnStateNode(Node.NodeState.Failure);
            FallbackNode chasePlayer = new FallbackNode(new List<Node>
            {
                rotate, engage, moveToTarget, pause
            });


            RootNode rootNode = new RootNode(chasePlayer);

            return rootNode;
        }
        
        
        #endregion
        

    }


}


     
/*            //1
            ConditionNode hasSighOnTargetCondition = new ConditionNode(() => hasTargetInSight);
            //2
            ConditionNode hasNoSightOnTargetCondition = new ConditionNode(() => !hasTargetInSight);
            //3
            ConditionNode hasNoOcclusionWithTargetCondition = new ConditionNode(() => hasTargetOcclusion);
            //4
            ConditionNode hasCloseRangeDistance = new ConditionNode(() => Vector3.Distance(position, target.position) < detection.data.closeRangeAttackDistance);
            //5
            ConditionNode hasLostTargetCondition = new ConditionNode(() => detection.targetRecentlyLost);*/
      