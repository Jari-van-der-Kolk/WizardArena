using DependencyInjection;
using Saxon.BT;
using Saxon.BT.AI.Controller;
using Saxon.BT.AI.Types;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;


public class AgentManager : MonoBehaviour, IDependencyProvider
{
    [Provide]
    public AgentManager ProvideAgentManager()
    {
        return this;
    }
    //config
    const int maxAgents = 10000;

    [Header("Agent update rate")]
    [Tooltip("The speed at which the behaviour tree updates")]
    [SerializeField] private float _BTUpdateStep = 0.5f;
    [Tooltip("The speed at which the object detection updates")]
    [SerializeField] private float _detectionUpdateStep = 0.5f;
    [SerializeField] private AgentController _agentControllerPrefab;

    public static AgentController agentControllerPrefab { get; private set; }

    //mutable
    private AgentControllerData[] _agentControllers = new AgentControllerData[maxAgents];

    [SerializeField] private int activeAgents;

    private void Awake()
    {
        agentControllerPrefab = _agentControllerPrefab;
    }

    private void Update()
    {
        for (int i = 0; i < activeAgents; i++)
        {
            _agentControllers[i].Poll(_BTUpdateStep, _detectionUpdateStep);
        }
    }

    public static void Spawn(Transform spawnLocation, AgentType agentType)
    {
        Instantiate(agentControllerPrefab, spawnLocation.position, Quaternion.identity);
    }

    //this method usually gets called during compile time
    public void Register(ref AgentControllerData agentController)
    {
        int index = SearchForUnallocatedSpot();
        Agent agent = AgentFactory.CreateAgent(agentController);
        agentController.Init(agent);
        agentController.SetAgentActivity(agentController.alive);

        if (index >= 0)
        {
            _agentControllers[index] = agentController;
        }

        activeAgents++;
    }

    // Private Methods
    public int SearchForUnallocatedSpot()
    {
        for (int i = 0; i < _agentControllers.Length; i++)
        {
            if (_agentControllers[i].enabled == false)
            {
                return i;
            }
        }

        Debug.LogWarning("No unallocated spot available for spawning new agent.");
        return -1; // Indicate no spot available
    }
}
