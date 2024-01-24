using UnityEngine.AI;
using Saxon.Sensor;
using Saxon.BT.AI.Controller;
using UnityEngine;
using Saxon.HashGrid;
using Saxon.NodePositioning;
using Saxon.BT.AI;

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

        public Vector3 position => agentController.transform.position;
        public float reachedLocationDistance = 3f;
        public bool hasTargetInSight => detection.hasTargetInSight;
        public bool hasReachedLocation => Saxon.IsInDistance(position, agentController.navMesh.destination, 3f);
        public SpatialHashGrid<HashNode> spatialHashGrid => NodeGenerator.Instance.hashGrid;


    
      

        public abstract BehaviourTree CreateTree();
        public void SetDestination(Vector3 destination) => navMesh.SetDestination(destination);
    }

}