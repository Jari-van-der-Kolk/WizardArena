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

namespace Saxon.BT.AI.Controller
{




    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentController : MonoBehaviour
    {
        [SerializeField] private AgentControllerData agentControllerData;



        [Inject]
        private AgentManager _agentManager;

        private void Start()
        {
            agentControllerData.InitComponents(this);
            _agentManager.Register(ref agentControllerData);
        }
        public void SetAgentActivity(bool activity)
        {
            agentControllerData.SetAgentActivity(activity);
        }

        public bool IsAlive()
        {
            return agentControllerData.alive;
        }

        public bool IsUpdatingRotation()
        {
            return agentControllerData.navMesh.updateRotation;
        }

        public void AddFollower(AgentControllerData follower)
        {
            if(agentControllerData.controllingAgents == null)
            {
                agentControllerData.controllingAgents = new List<AgentControllerData>();
            }

            agentControllerData.controllingAgents.Add(follower);
        }

        public AgentControllerData GetData() => agentControllerData;

    }

}

[System.Serializable]
public struct AgentControllerData
{
    [Header("Config")]
    
    [OnValueChanged("OnValueChangedMethod1")]
    public AgentType agentType;
    private Agent agent;

    public ActionRegister actionRegister;
    public FloatReference maxHealth;

    public ObjectDetection objectDetection { get; private set; }
    public Transform transform { get; private set; }
    public NavMeshAgent navMesh;

    public Rigidbody rb { get; private set; }
    public Collider col { get; private set; }

    public void Init(Agent agent)
    {
        this.agent = agent;
        enabled = true;
    }

    public void InitComponents(MonoBehaviour monoBehaviour)
    {
        transform = monoBehaviour.transform;
        objectDetection = monoBehaviour.GetComponent<ObjectDetection>();
        navMesh = monoBehaviour.GetComponent<NavMeshAgent>();
        rb = monoBehaviour.GetComponent<Rigidbody>();
        col = monoBehaviour.GetComponent<Collider>();

        navMesh.updateRotation = false;
    }

    [Header("Mutable")]
    public bool enabled;
    public bool alive;
    public Transform origin => agent.origin;
    [SerializeField] private int _health;

    public List<AgentControllerData> controllingAgents;

    public void Poll(float BTUpdateStep, float ODUpdateStep)
    {
        if (alive)
        {
            objectDetection.TimeStepUpdate(ODUpdateStep);
            agent.TimeStepUpdate(BTUpdateStep);
        }
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
        rb.useGravity = !agentStatus;

        return agentStatus;
    }

    public void Revive()
    {
        alive = true;
        rb.useGravity = false;
        col.isTrigger = true;
    }

    public bool IsAlive()
    {
        return alive;
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