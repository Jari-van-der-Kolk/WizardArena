using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saxon.Sensor;
using Saxon.BT;
using Saxon.BT.AI;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentController : MonoBehaviour
{
    [SerializeField] private AgentTypes enemyType;
    [SerializeField] private ObjectDetectionData detectionData;
    [Tooltip("The speed at which the behaviour tree updates")]
    [SerializeField] private float BTStepDuration = 0.5f;
    [Tooltip("The speed at which the object detection updates")]
    [SerializeField] private float ODStepDuration = 0.5f;

    RootNode behaviourTree;
    Agent currentAgent;
    Dictionary<AgentTypes, Saxon.BT.Agent> enemyTypes;
    
    internal NavMeshAgent navMesh;
    public ObjectDetection objectDetection;


    #region debug
    private void OnValidate()
    {
        objectDetection = new ObjectDetection(transform, detectionData);
        objectDetection.Validate();
    }

    private void OnDrawGizmos()
    {
        objectDetection.Debug();
    }

    #endregion

    private void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        objectDetection = new ObjectDetection(transform, detectionData);
        currentAgent = AgentFactory();
        behaviourTree = currentAgent.CreateTree();
    }

    void Update()
    {
        behaviourTree.TimeStepUpdate(BTStepDuration); 
        objectDetection.TimeStepUpdate(ODStepDuration); 
    }


    private Agent AgentFactory()
    {
        Dictionary<AgentTypes, Agent> enemyTypes = new Dictionary<AgentTypes, Saxon.BT.Agent>
        {
            { AgentTypes.CloseRangeWizard, new CloseRangeWizard(this) },
            { AgentTypes.MidRangeWizard, new MidRangeWizard(this) },
            { AgentTypes.LongRangeWizard, new LongRangeWizard(this) },
        };

        return enemyTypes[enemyType];
    }
}
