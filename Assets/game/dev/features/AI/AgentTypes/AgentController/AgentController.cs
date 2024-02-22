using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Saxon.Sensor;
using System.Linq;

namespace Saxon.BT.AI
{
    public enum AgentTypes
    {
        CloseRangeWizard,
        MidRangeWizard,
        LongRangeWizard,
        Necromancer,
        NecroServant,
        Spider,
    };
}

namespace Saxon.BT.AI.Controller
{
    //TODO DEBUG//
    //TODO de necrospell zijn delay decorator WaitConditionNode geeft geen delay

    //TODO//
    //TODO zorg ervoor dat er een priority system komt voor het uitkiezen van targets
    
    //TODO maak een notification system dat in de gaten houd welke actie die moet uivoeren als er iets in de buurt word gedaan of gebeurd,
    //  bijv geluid/kabaal,- beschoten worden,- ect

    //TODO zorg ervoor dat de necroservant een random plaats rondom de necromancer vind met behulp van de spacial hash grid


    

    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentController : MonoBehaviour
    {
        [SerializeField] private AgentTypes _agentType;
        [SerializeField] private ObjectDetectionData _detectionData;
        [Tooltip("The speed at which the behaviour tree updates")]
        [SerializeField] private float _BTUpdateStep = 0.5f;
        [Tooltip("The speed at which the object detection updates")]
        [SerializeField] private float _detectionUpdateStep = 0.5f;

        public Vector3 destination { get; private set; }
        public Transform origin { get; set; }
        public List<Transform> waypoints;
        public ObjectDetection objectDetection;
        public bool alive = true;
        internal NavMeshAgent navMesh;
    
        Agent currentAgent;
        BehaviourTree behaviourTree;
        Rigidbody rb;
        Collider col;

        #region debug
#if UNITY_EDITOR

        [Space]
        [SerializeField] private bool hasTargetInSight;
        [SerializeField] private bool isTargetRecentlyLost;
        [SerializeField] private bool lostTarget;
        [SerializeField] private bool occlusion;
        [SerializeField] private bool navmeshRotate;
        [SerializeField] private Transform target;
        [SerializeField] private bool debug;
        [SerializeField] private bool debugAttackRanges;

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
                if(debugAttackRanges)
                {
                    objectDetection.DrawAttackRanges();
                }

            }
        }

        private void DebugVariables()
        {
            lostTarget = objectDetection.lostTarget;
            hasTargetInSight = objectDetection.hasTargetInSight;
            isTargetRecentlyLost = objectDetection.targetRecentlyLost;
            occlusion = currentAgent.hasTargetOcclusion;
            target = objectDetection.target;
            navmeshRotate = navMesh.updateRotation;
        }

    #endif
#endregion
        void Awake()
        {
            navMesh = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();
        }
    
        //var foo = FindObjectsOfType<GameObject>().Where(obj => obj.gameObject.layer == objectDetection.data.VieldOfViewLayers).ToList();
        void Start()
        {

            SetAgentActivity(alive);

            //dont change the order of currentAgent and behaviourTree otherwise the debugger will start bitching 
            objectDetection = new ObjectDetection(transform, _detectionData);
            SetAgentType(_agentType);
            SetDestination(transform.position);
#region Editor
    #if UNITY_EDITOR
            objectDetection.Validate();
#endif
            #endregion
        }
    
        void Update()
        {
            if (alive)
            {
                objectDetection.TimeStepUpdate(_detectionUpdateStep); 
                behaviourTree.TimeStepUpdate(_BTUpdateStep);
            }
            else
            {
                SetDestination(destination);
            }

            
            DebugVariables();

        }
        public void SetAgentType(AgentTypes agentType)
        {
            currentAgent = AgentFactory(agentType);
            behaviourTree = currentAgent.CreateTree();
            _agentType = agentType;
        }

        public void SetDestination(Vector3 destination)
        {
            this.destination = destination;
        }

        //TODO make sure the agent can die and come alive again
        public bool SetAgentActivity(bool agentStatus)
        {
            alive = agentStatus;
            navMesh.enabled = agentStatus;
            rb.useGravity = !agentStatus;
            col.isTrigger = agentStatus;

            return agentStatus;
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
                case AgentTypes.NecroServant:
                    agent = new NecroServant(this);
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