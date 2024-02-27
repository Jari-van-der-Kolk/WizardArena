using UnityEngine;
using UnityEngine.AI;
using Saxon.HashGrid;
using Saxon.NodePositioning;
using Saxon.Sensor;
using Saxon.BT.AI;
using Saxon.BT.AI.Controller;
using System.Collections.Generic;

namespace Saxon.BT
{
    public abstract class Agent
    {
        public Agent(AgentController agentController)
        {
            this.agentController = agentController;
            origin = agentController.transform;
        }
        public abstract AgentTypes agentType { get; protected set; }
        public AgentController agentController;
        public NavMeshAgent navMesh => agentController.navMesh;
        public ObjectDetection detection => agentController.objectDetection;
        public Transform origin { get; set; }
        public RootNode rootNode { get; protected set; }
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

        public void SetOrigin(Transform origin)
        {
            this.origin = origin;
        }

        #region Nodes

        public Node ChaseTarget(float reachedTargetDistance)
        {
            MoveTowardsTargetNode moveTowardsTarget = new MoveTowardsTargetNode(this, reachedTargetDistance);
            RotateTowardsTargetNode rotateTowardsTarget = new RotateTowardsTargetNode(this, 2f);

            SelectorNode lookAtTarget = new SelectorNode(new List<Node>
            {
                hasNoOcclusion, InRangeOfTarget(detection.data.longRangeAttackDistance) 
            });

            SequenceNode rotate = new SequenceNode(new List<Node>
            {
                lookAtTarget, rotateTowardsTarget
            });

            ParallelNode engage = new ParallelNode(new List<Node>
            {
                moveTowardsTarget, rotate    
            });

            SequenceNode moveToTarget = new SequenceNode("move",new List<Node>
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
        public Node InRangeOfTarget(float range) => new ConditionNode(() => Saxon.IsInDistance(position, target, range));
        public Node recentlyLostTarget => new ConditionNode(() => detection.targetRecentlyLost);
        public Node FoundTarget()
        {
            return new SequenceNode(new List<Node>
            {
                targetInSight, InRangeOfTarget(detection.data.closeRangeAttackDistance)
            });
        }
        public Node lostTarget => new ConditionNode(() => detection.lostTarget);
        
        #endregion

    }


}
