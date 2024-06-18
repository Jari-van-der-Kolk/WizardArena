using UnityEngine;
using UnityEngine.AI;
using DependencyInjection;
using Saxon.BT.AI.Types;
using Saxon.HashGrid;
using Saxon.NodePositioning;
using System.Collections.Generic;
using Saxon.BT;
using RoboRyanTron.Unite2017.Variables;
using Unity.Collections;
using Unity.Jobs;
using NaughtyAttributes;
using Saxon.BT.AI.Controller;
using PlasticGui.WorkspaceWindow.PendingChanges;

#region UnusedCode
//var foo = FindObjectsOfType<GameObject>().Where(obj => obj.gameObject.layer == objectDetection.data.VieldOfViewLayers).ToList();
/* objectDetection = new ObjectDetection(transform, _detectionData);

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

namespace Saxon.BT.AI.Controller
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentBehaviour : MonoBehaviour, IOwner, IFollower
    {
        [SerializeField] private AgentBehaviourData agentData;
        public int agentIndex;

        [Inject]
        private AgentManager _agentManager;

        private void Start()
        {
            agentData.Init(this);
            _agentManager.Register(ref agentData, ref agentIndex);
        }

        public ref AgentBehaviourData data => ref _agentManager.GetAgentControllerDataByRef(agentIndex);


        public bool GetDetection()
        {
            return agentData.objectDetection.hasTargetInSight;
        }

        [Button(enabledMode: EButtonEnableMode.Always)]
        public void UpdateData()
        {
            agentData = _agentManager.GetAgentControllerData(agentIndex);
        }        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public void UpdateAlive()
        {
            data.Revive();
            UpdateData();
        }

       
    }



}

[System.Serializable]
public struct AgentBehaviourData
{
    [Header("Config")]
    private Agent agent;
    public AgentType agentType;
    public ActionRegister actionRegister;
    public FloatReference maxHealth;
    [Tag] public string targetTag;

    public MonoBehaviour monoBehaviour { get; private set; }
    public ObjectDetection objectDetection { get; private set; }
    public Transform transform { get; private set; }
    public FollowersHolder followersHolder { get; private set; }
    public NavMeshAgent navMesh { get; private set; }
    public Rigidbody rb { get; private set; }
    public Collider col { get; private set; }


    public void SetAgent(Agent agent)
    {
        this.agent = agent;
    }

    public void Init(MonoBehaviour monoBehaviour)
    {
        this.monoBehaviour = monoBehaviour;
        transform = monoBehaviour.transform;
        objectDetection = monoBehaviour.GetComponent<ObjectDetection>();
        followersHolder = monoBehaviour.GetComponent<FollowersHolder>();
        navMesh = monoBehaviour.GetComponent<NavMeshAgent>();
        rb = monoBehaviour.GetComponent<Rigidbody>();
        col = monoBehaviour.GetComponent<Collider>();

        enabled = true;
    }

    [Header("Mutable")]
    [SerializeField]public bool enabled;
    public bool alive;
    public int _health {  get; private set; }

    
    public void Poll()
    {
        if (alive)
        {
            objectDetection.UpdataOD();
            agent.TimeStepUpdate();
        }
    }

    public void ChangeAgentType(AgentType agentType)
    {
        SetAgentType(agentType);
        SetAgent(AgentFactory.CreateAgent(this));
    }

    public void SetAgentType(AgentType agentType)
    {
        this.agentType = agentType;
    }

    public void SetOrigin(Transform origin)
    {
        agent.origin = origin;
    }

    public bool SetAgentActivity(bool agentStatus)
    {
        alive = agentStatus;
        col.isTrigger = agentStatus;
        navMesh.enabled = agentStatus;

        if (!agentStatus)
        {
            rb.velocity = navMesh.velocity;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

        rb.useGravity = !agentStatus;

        return agentStatus;
    }

    public void Revive()
    {


        alive = true;
        col.isTrigger = true;
        navMesh.enabled = true;
        rb.useGravity = false;

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