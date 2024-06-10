using UnityEngine;
using UnityEngine.AI;
using Saxon.HashGrid;
using Saxon.NodePositioning;
using Saxon.BT.AI.Types;
using Saxon.BT.AI.Controller;
using System.Collections.Generic;

namespace Saxon.BT
{
    public abstract class Agent
    {
        public abstract BehaviourTree CreateTree();
        public abstract AgentType agentType { get; protected set; }
        public Agent(AgentControllerData agentData)
        {
            this.agentData = agentData;
        }
        public AgentControllerData agentData;

        public ObjectDetection detection => agentData.objectDetection;
        public Transform origin => agentData.origin;
        public RootNode rootNode { get; protected set; }
        public Transform target => detection.target;
        public Transform transform => agentData.transform;
        public Vector3 position => agentData.transform.position;
        public float reachedLocationDistance = 3f;
        public bool isAgentAtDestination => agentData.navMesh.remainingDistance <= agentData.navMesh.stoppingDistance;
        public bool hasTargetInSight => detection.hasTargetInSight;
        public bool hasTargetOcclusion => detection.HasOcclusionWithTarget();
        public SpatialHashGrid<HashNode> spatialHashGrid => NodeGenerator.Instance.hashGrid;
        public void SetDestination(Vector3 destination) => agentData.navMesh.SetDestination(destination);
        public bool IsInDistance(Vector3 origin, Vector3 target, float inDistanceLength)
        {
            return Vector3.Distance(origin, target) < inDistanceLength;
        }
        public bool IsInDistance(Vector3 origin, Transform target, float inDistanceLength)
        {
            if (target == null)
                return false;
            return Vector3.Distance(origin, target.position) < inDistanceLength;
        }

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

        public Node ChaseTarget(float reachedTargetDistance)
        {
            MoveTowardsTargetNode moveTowardsTarget = new MoveTowardsTargetNode(this, reachedTargetDistance);
            RotateTowardsTargetNode rotateTowardsTarget = new RotateTowardsTargetNode(this, 2f);

            SelectorNode lookAtTarget = new SelectorNode(new Node[]
            {
                hasNoOcclusion, InRangeOfTarget(detection.data.longRangeAttackDistance) 
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


        public Node targetInSight => new ConditionNode(() => hasTargetInSight);
        public Node TargetOutOfSight => new ConditionNode(() => detection.noVisualsOnTarget);
        public Node hasOcclusion => new ConditionNode(() => hasTargetOcclusion);
        public Node hasNoOcclusion => new ConditionNode(() => !hasTargetOcclusion);
        public Node InRangeOfTarget(float range) => new ConditionNode(() => IsInDistance(position, target, range));
        public Node recentlyLostTarget => new ConditionNode(() => detection.targetRecentlyLost);
        public Node FoundTarget()
        {
            return new SequenceNode(new Node[]
            {
                targetInSight, InRangeOfTarget(detection.data.closeRangeAttackDistance)
            });
        }
        public Node lostTarget => new ConditionNode(() => detection.lostTarget);
        
        #endregion

    }


}
