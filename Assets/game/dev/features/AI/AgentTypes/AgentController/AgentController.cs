using UnityEngine;
using UnityEngine.AI;
using DependencyInjection;
using Saxon.BT.AI.Types;

namespace Saxon.BT.AI.Controller
{
    #region UnusedCode
    //var foo = FindObjectsOfType<GameObject>().Where(obj => obj.gameObject.layer == objectDetection.data.VieldOfViewLayers).ToList();
    /* objectDetection = new ObjectDetection(transform, _detectionData);
            SetAgentActivity(alive);

            //dont change the order of currentAgent and behaviourTree otherwise the debugger will start bitching 
            SetAgentType(_agentType);
            SetDestination(transform.position);
*/
    /* public AgentController SetAgentType(AgentType agentType)
         {
             currentAgent = _agentFactory.CreateType(agentType, this);
             behaviourTree = currentAgent.CreateTree();
             _agentType = agentType;

             return this;
         }

         public AgentController SetAgent(Agent agent)
         {
             currentAgent = agent;
             return this;
         }

         public AgentController Build()
         {
             behaviourTree = currentAgent.CreateTree();  
             return this;
         }*/

    #endregion

    //TODO DEBUG//
    //TODO de necrospell zijn delay decorator WaitConditionNode geeft geen delay

    //TODO//
    //TODO zorg ervoor dat er een priority system komt voor het uitkiezen van targets

    //TODO maak een notification system dat in de gaten houd welke actie die moet uivoeren als er iets in de buurt word gedaan of gebeurd,
    //  bijv geluid/kabaal,- beschoten worden,- ect

    //TODO zorg ervoor dat de necroservant een random plaats rondom de necromancer vind met behulp van de spacial hash grid



    //################# 

    //  Init does not get called!!!!!!!!!!!!!!!!!!!!


    //#################

    [System.Serializable]
    public class AgentControllerData
    {
        [Header("Config")]
        public AgentType agentType;
        public Agent currentAgent;
        public ObjectDetectionData detectionData;
        public ObjectDetection objectDetection;
        public BehaviourTree behaviourTree;

        public Transform transform {  get; private set; }
        public SpellCaster spellCaster {  get; private set; }
        public NavMeshAgent navMesh {  get; private set; }
        public Rigidbody rb { get; private set; }
        public Collider col {  get; private set; }


        public void SetComponents(MonoBehaviour monoBehaviour)
        {
            transform = monoBehaviour.transform;
            spellCaster = monoBehaviour.GetComponent<SpellCaster>();    
            navMesh = monoBehaviour.GetComponent<NavMeshAgent>();
            rb = monoBehaviour.GetComponent<Rigidbody>();
            col = monoBehaviour.GetComponent<Collider>();
        }
       

        [Header("Mutable")]
        public bool alive = true;
        public bool active;
        public Vector3 destination { get; private set; }
        public Transform origin {  get; private set; }

        public void SetOrigin(Transform origin)
        {
            transform = origin;
        }

        public void SetDestination(Vector3 destination)
        {
            this.destination = destination;
        }

        public void SetLifeStatus(bool lifeStatus)
        {
            active = lifeStatus;
        }

    }


    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentController : MonoBehaviour
    {
        [SerializeField] private AgentControllerData agentData;
        
        [Inject]
        private AgentManager agentManager;

        [Inject]
        private AgentFactory _agentFactory;

        #region debug
#if UNITY_EDITOR

        [SerializeField] private DebugAgentControllerData _debugAgentData;


        private void OnValidate()
        {
            agentData.SetComponents(this);
            agentData.objectDetection = new ObjectDetection(agentData);
            agentData.objectDetection.Validate();
        }

        private void OnDrawGizmos()
        {
            if (_debugAgentData.debug)
            {
                agentData.objectDetection.DrawGizmo();
                if (_debugAgentData.showDecisionRanges)
                {
                    agentData.objectDetection.DrawAttackRanges();
                }

            }
        }

        private void DebugVariables()
        {
            _debugAgentData.lostTarget = agentData.objectDetection.lostTarget;
            _debugAgentData.hasTargetInSight = agentData.objectDetection.hasTargetInSight;
            _debugAgentData.isTargetRecentlyLost = agentData.objectDetection.targetRecentlyLost;
            _debugAgentData.occlusion = agentData.currentAgent.hasTargetOcclusion;
            _debugAgentData.target = agentData.objectDetection.target;
            _debugAgentData.navmeshRotate = agentData.navMesh.updateRotation;
        }

#endif
        #endregion


        void Awake()
        {
            agentData.SetComponents(this); 
        }
    
        public void Init()
        {
            _agentFactory.director.Construct(agentData);
            agentManager.Register(this);
           
            #region Editor
#if UNITY_EDITOR
            agentData.objectDetection.Validate();
#endif
            #endregion
        }
    
        public void Poll(float BTUpdateStep, float ODUpdateStep)
        {
            if (agentData.alive)
            {
                agentData.objectDetection.TimeStepUpdate(ODUpdateStep);
                agentData.behaviourTree.TimeStepUpdate(BTUpdateStep);
            }
            else
            {
                agentData.SetDestination(transform.position);
            }

#if UNITY_EDITOR
            DebugVariables();
#endif

        }
       
        //TODO make sure the agent can die and come alive again
        public bool SetAgentActivity(bool agentStatus)
        {
            agentData.alive = agentStatus;
            agentData.navMesh.enabled = agentStatus;
            agentData.rb.useGravity = !agentStatus;
            agentData.col.isTrigger = agentStatus;

            return agentStatus;
        }

        public bool IsAlive()
        {
            return agentData.alive;
        }

       



    }

}