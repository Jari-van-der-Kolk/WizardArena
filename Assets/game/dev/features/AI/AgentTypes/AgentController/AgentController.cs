using UnityEngine;
using UnityEngine.AI;
using DependencyInjection;
using Saxon.BT.AI.Types;
using Saxon.HashGrid;
using Saxon.NodePositioning;
using System.Collections.Generic;

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
    public struct AgentControllerData
    {
        [Header("Config")]
        public AgentType agentType;
        private Agent agent;

        public ObjectDetection objectDetection;
        public Transform transform {  get; private set; }
        public Health health { get; private set; }
        public SpellCaster spellCaster {  get; private set; }
        public NavMeshAgent navMesh {  get; private set; }
        public Rigidbody rb { get; private set; }
        public Collider col {  get; private set; }

        public void Init(Agent agent)
        {
            this.agent = agent;

        }
        public void SetComponents(MonoBehaviour monoBehaviour)
        {
            transform = monoBehaviour.transform;
            objectDetection = monoBehaviour.GetComponent<ObjectDetection>();    
            spellCaster = monoBehaviour.GetComponent<SpellCaster>();    
            navMesh = monoBehaviour.GetComponent<NavMeshAgent>();
            rb = monoBehaviour.GetComponent<Rigidbody>();
            col = monoBehaviour.GetComponent<Collider>();
        }

        [Header("Mutable")]
        public bool enabled;
        public bool alive;
        public bool active;
        public Vector3 destination;
        public Transform origin;

        public void Poll(float BTUpdateStep, float ODUpdateStep)
        {
            if (alive)
            {
                objectDetection.TimeStepUpdate(ODUpdateStep);
                agent.TimeStepUpdate(BTUpdateStep);
            }
            else
            {
                SetDestination(transform.position);
            }
        }

            public void SetOrigin(Transform origin)
        {
            this.origin = origin;
        }

        public void SetDestination(Vector3 destination)
        {
            this.destination = destination;
        }

        public void SetLifeStatus(bool lifeStatus)
        {
            active = lifeStatus;
        }

        public bool SetAgentActivity(bool agentStatus)
        {
            alive = agentStatus;
            rb.useGravity = !agentStatus;
            col.isTrigger = agentStatus;

            return agentStatus;
        }

        public bool IsAlive()
        {
            return alive;
        }

        
    }

    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentController : MonoBehaviour
    {
        [SerializeField] private AgentControllerData _agentControllerData;

        public Agent agent { get; private set; }
        public ObjectDetection objectDetection { get; private set; }

        [Inject]
        private AgentManager _agentManager;

        [Inject]
        private BehaviourTreeFactory _agentFactory;

        private void Start()
        {
            _agentControllerData.SetComponents(this);
            _agentManager.Register(_agentControllerData);
            
        }

        public void SetAgentActivity(bool activity)
        {
            _agentControllerData.alive = activity;
            _agentControllerData.rb.useGravity = !activity;
            _agentControllerData.col.isTrigger = activity;
        }

        public bool IsAlive()
        {
            return _agentControllerData.alive;
        }

    }

}
            //_agentControllerData.SetAgentData(agent).SetObjectDetectionData(objectDetection);
/*        void Awake()
        {
            agentControllerData.SetComponents(this);
        }

        public void Init()
        {
            _agentFactory.director.Construct(agentControllerData);
            agentManager.Register(this);

            #region Editor
#if UNITY_EDITOR
            agentControllerData.objectDetection.Validate();
#endif
            #endregion
        }

        #region debug
#if UNITY_EDITOR

        [SerializeField] private DebugAgentControllerData _debugAgentData;

        private void OnValidate()
        {
            agentControllerData.SetComponents(this);
            agentControllerData.objectDetection = new ObjectDetection(agentControllerData);
            agentControllerData.objectDetection.Validate();
        }

        private void OnDrawGizmos()
        {
            if (_debugAgentData.debug)
            {
                agentControllerData.objectDetection.DrawGizmo();
                if (_debugAgentData.showDecisionRanges)
                {
                    agentControllerData.objectDetection.DrawAttackRanges();
                }

            }
        }

        private void DebugVariables()
        {
            _debugAgentData.lostTarget = agentControllerData.objectDetection.lostTarget;
            _debugAgentData.hasTargetInSight = agentControllerData.objectDetection.hasTargetInSight;
            _debugAgentData.isTargetRecentlyLost = agentControllerData.objectDetection.targetRecentlyLost;
            _debugAgentData.occlusion = agentControllerData.agent.hasTargetOcclusion;
            _debugAgentData.target = agentControllerData.objectDetection.target;
            _debugAgentData.navmeshRotate = agentControllerData.navMesh.updateRotation;
        }

#endif
        #endregion


    
      */