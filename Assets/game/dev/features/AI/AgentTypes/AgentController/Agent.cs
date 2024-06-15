using UnityEngine;
using UnityEngine.AI;
using Saxon.HashGrid;
using Saxon.NodePositioning;
using Saxon.BT.AI.Types;
using System.Collections.Generic;

namespace Saxon.BT
{
    public abstract class Agent
    {
        public Agent(AgentControllerData agentData)
        {
            origin = agentData.transform;
            this.agentData = agentData;
            startTime = Time.time;
        }
        public AgentControllerData agentData;
        public abstract RootNode CreateTree();
        public abstract AgentType agentType { get; protected set; }
        public abstract RootNode rootNode { get; protected set; }
        
        private float startTime;
        public SpatialHashGrid<HashNode> spatialHashGrid => NodeGenerator.Instance.hashGrid;

        public Transform origin;

        //Object Detection ShortCuts
        public ObjectDetection detection => agentData.objectDetection;
        public Transform target => detection.target;
        //AgentData ShortCuts
        public NavMeshAgent navMesh => agentData.navMesh;   
        public Transform transform => agentData.transform;
        public Vector3 position => agentData.transform.position;

        public void TimeStepUpdate(float timestep)
        {
            float time = Time.time;
            if (time - startTime > timestep)
            {
                rootNode.SetDeltaTime(time - startTime);
                rootNode.Update();
                startTime = time;
            }
        }


        //Method ShortCuts
        public void SetDestination(Vector3 destination) => agentData.navMesh.SetDestination(destination);
        public bool IsInDistance(Vector3 origin, Vector3 target, float inDistanceLength)
        {
            return Vector3.Distance(origin, target) < inDistanceLength;
        }
        public bool CheckServantsDetection(List<AgentControllerData> agents)
        {
            if (agents.Count > 0)
            {
                for (int i = 0; i < agents.Count; i++)
                {
                    if (agents[i].objectDetection.hasTargetInSight)
                    {
                        var target = agents[i].objectDetection.target;
                        detection.SetTarget(target);

                        for (int c = 0; c < agents.Count; c++)
                        {
                            agents[c].objectDetection.ToggleTargetRecentlyLost(true);
                            agents[c].objectDetection.ResetRecentlyLostTimer();
                            agents[c].objectDetection.SetTarget(target);
                        }

                        return true;
                    }

                }
            }
            return false;
        }
       

        #region Nodes

        public Node ChaseTarget(float reachedTargetDistance)
        {

            MoveTowardsTargetNode moveTowardsTarget = new MoveTowardsTargetNode(this, reachedTargetDistance);
            RotateTowardsTargetNode rotateTowardsTarget = new RotateTowardsTargetNode(this, 2f);

            SelectorNode lookAtTarget = new SelectorNode(new Node[]
            {
                hasNoOcclusion, InRangeOfTarget(detection.Data.longRangeAttackDistance) 
            });

            SequenceNode rotate = new SequenceNode(new Node[]
            {
                lookAtTarget, rotateTowardsTarget
            });

            ParallelNode engage = new ParallelNode(new Node[]
            {
                moveTowardsTarget, rotate    
            });

            SequenceNode moveToTarget = new SequenceNode("move",new Node[]
            {
                engage       
            });

 
            RootNode rootNode = new RootNode(moveToTarget);

            return rootNode;
        }

        public Node targetInSight => new ConditionNode(() => detection.hasTargetInSight);
        public Node TargetOutOfSight => new ConditionNode(() => detection.noVisualsOnTarget);
        public Node hasOcclusion => new ConditionNode(() => detection.HasOcclusionWithTarget());
        public Node hasNoOcclusion => new ConditionNode(() => !detection.HasOcclusionWithTarget());
        public Node InRangeOfTarget(float range) => new ConditionNode(() => IsInDistance(position, target.position, range));
        public Node recentlyLostTarget => new ConditionNode(() => detection.targetRecentlyLost);
        public Node FoundTarget()
        {
            return new SequenceNode(new Node[]
            {
                targetInSight, InRangeOfTarget(detection.Data.closeRangeAttackDistance)
            });
        }
        public Node lostTarget => new ConditionNode(() => detection.lostTarget);
        
        #endregion

    }


}


//Misc
//public bool isAgentAtDestination => agentData.navMesh.remainingDistance <= agentData.navMesh.stoppingDistance;