using UnityEngine.AI;
using Saxon.Sensor;
using Saxon.BT.AI.Controller;
using UnityEngine;
using Saxon.HashGrid;
using Saxon.NodePositioning;
using Saxon.BT.AI;
using System.Collections.Generic;
using System;

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
            return new ConditionNode("occlusion",() => hasTargetOcclusion);
        }

        public Node HasNoOcclusion()
        {
            return new ConditionNode("occlusion", () => !hasTargetOcclusion);
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
            return new SequenceNode(new List<Node>
            {
                TargetOutOfSight(), InRangeOfTarget(detection.data.midRangeAttackDistance) 
            });
        }

        public Node ChasePlayer(float reachedTargetDistance)
        {
            MoveToDestinationCommand standStillCommand = new MoveToDestinationCommand(navMesh, detection);
            ExecuteCommandNode standStill = new ExecuteCommandNode(this, standStillCommand);

            RotateTowardsCommand rotateTowardsTargetCommand = new RotateTowardsCommand(transform, detection);
            ExecuteCommandNode rotateTowardsTarget = new ExecuteCommandNode(this, rotateTowardsTargetCommand, false);

            MoveToDestinationCommand moveTowardsTargetCommand = new MoveToDestinationCommand(navMesh, detection, detection.data.longRangeAttackDistance);
            ExecuteCommandNode moveTowardsTarget = new ExecuteCommandNode(this, moveTowardsTargetCommand);

            
            /*SequenceNode engage = new SequenceNode("engage",new List<Node>
            {
                RecentlyLostTarget(), HasOcclusion(), InRangeOfTarget(detection.data.midRangeAttackDistance), moveTowardsTarget, rotateTowardsTarget
            });*/


            SequenceNode found = new SequenceNode("found", new List<Node>
            {
                TargetInSight(), InRangeOfTarget(reachedTargetDistance), standStill     
            });


            SequenceNode adjust = new SequenceNode("adjust",new List<Node>
            {
                HasOcclusion(), RecentlyLostTarget(), InRangeOfTarget(detection.data.longRangeAttackDistance),rotateTowardsTarget                    
            });   

            SequenceNode engage = new SequenceNode(new List<Node>
            {
                moveTowardsTarget, rotateTowardsTarget
            });

            FallbackNode chasePlayer = new FallbackNode(new List<Node>
            {
                engage, found, adjust
            });


            RootNode rootNode = new RootNode(adjust);

            return rootNode;
        }
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
      