using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Saxon.Sensor;

namespace Saxon.BT.AI
{
    public enum AgentTypes
    {
        CloseRangeWizard,
        MidRangeWizard,
        LongRangeWizard,
        Necromancer,
        Spider,
    };
}

namespace Saxon.BT.AI.Controller
{

    //TODO zorg ervoor dat er in de ObjectDetection class de bool genaamt lostTarget komt 

    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentController : MonoBehaviour
    {
        [SerializeField] private AgentTypes _enemyType;
        [SerializeField] private ObjectDetectionData _detectionData;
        [Tooltip("The speed at which the behaviour tree updates")]
        [SerializeField] private float _BTUpdateStep = 0.5f;
        [Tooltip("The speed at which the object detection updates")]
        [SerializeField] private float _detectionUpdateStep = 0.5f;

        internal NavMeshAgent navMesh;
    
        public ObjectDetection objectDetection;
        public List<Transform> waypoints;
    
        BehaviourTree behaviourTree;
        Agent currentAgent;

        bool alive;

#if UNITY_EDITOR
        #region debug

        [SerializeField] private bool hasTargetInSight;
        [SerializeField] private bool isTargetRecentlyLost;
        [SerializeField] private bool debug;

        private void OnValidate()
        {
            objectDetection = new ObjectDetection(transform, _detectionData);
            objectDetection.Validate();
        }

        private void OnDrawGizmos()
        {
            if (debug)
            {
                objectDetection.DrawGizmo();
            }
        }

        private void DebugVariables()
        {
            hasTargetInSight = objectDetection.hasTargetInSight;
            isTargetRecentlyLost = objectDetection.targetRecentlyLost;
        }


        #endregion
#endif
        void Awake()
        {
            navMesh = GetComponent<NavMeshAgent>();
        }
    
        void Start()
        {
            alive = true;

            //dont change the order of currentAgent and behaviourTree otherwise the debugger will start bitching 
            objectDetection = new ObjectDetection(transform, _detectionData);

#if UNITY_EDITOR
            objectDetection.Validate();
#endif

            currentAgent = AgentFactory(_enemyType);
            behaviourTree = currentAgent.CreateTree();
        }
    
        void Update()
        {
            if (alive)
            {
                objectDetection.TimeStepUpdate(_detectionUpdateStep); 
                behaviourTree.TimeStepUpdate(_BTUpdateStep);
            }

#if UNITY_EDITOR
            DebugVariables();
#endif
        }

        //TODO make sure the agent can die and come alive again
        public void SetAgentStatus(bool agentStatus)
        {
            alive = agentStatus;
            enabled = agentStatus;
            navMesh.enabled = agentStatus;
            
        }

        Agent AgentFactory(AgentTypes agentType)
        {
            Agent agent = null;
    
            switch (agentType)
            {
                case AgentTypes.CloseRangeWizard:
                    agent = new CloseRangeWizard(this);
                    break;
                case AgentTypes.MidRangeWizard:
                    agent = new MidRangeWizard(this);
                    break;
                case AgentTypes.LongRangeWizard:
                    agent = new LongRangeWizard(this);
                    break;
                case AgentTypes.Necromancer:
                    agent = new Necromancer(this);
                    break;
                case AgentTypes.Spider:
                    agent = new Spider(this);
                    break;
                // Add more cases as needed
                default:
                    Debug.Log("You might want to assign the: " + agentType + 
                        " Inside of the Factory method found inside of: " + gameObject);
                    break;
            }
    
            return agent;
        }
    
    }

}