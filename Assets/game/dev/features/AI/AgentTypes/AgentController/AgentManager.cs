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
using System;

public class AgentManager : MonoBehaviour, IDependencyProvider
{
    [Provide]
    public AgentManager ProvideAgentManager()
    {
        return this;
    }
    //config
    const int maxAgents = 100;

    [Header("Agent update rate")]
    [SerializeField] private float _detectionUpdateStep = 0.5f;
    [SerializeField] private AgentBehaviour _agentControllerPrefab;

    public static AgentBehaviour agentControllerPrefab { get; private set; }

    //mutable
    [SerializeField] private AgentBehaviourData[] _agentControllers = new AgentBehaviourData[maxAgents];
    [SerializeField] private int activeAgents;


    private void Awake()
    {
        agentControllerPrefab = _agentControllerPrefab;
    }

    private void Update()
    {
        for (int i = 0; i < activeAgents; i++)
        {
            _agentControllers[i].Poll();
        }
    }

    public void Spawn(Transform spawnLocation, AgentType agentType)
    {
        int index = SearchForUnallocatedSpot();
        if (index == -1)
        {
            Debug.LogWarning("No unallocated spot available for spawning new agent.");
            return;
        }

        // Instantiate the agent controller and set its index
        AgentBehaviourData agentControllerData = new AgentBehaviourData();
        agentControllerData.agentType = agentType;  
        _agentControllers[index] = agentControllerData;

        AgentBehaviour agentController = Instantiate(agentControllerPrefab, spawnLocation.position, Quaternion.identity);
        agentController.data = _agentControllers[index];

        // Initialize the AgentControllerData
    }

    //this method usually gets called during compile time
    public int Register(ref AgentBehaviourData data, ref int index)
    {
        index = SearchForUnallocatedSpot();
        if(index > maxAgents)
        {
            Debug.LogWarning("You have not allocated enough memory to initialize more agents");
            return -1;
        }


        Agent agent = AgentFactory.CreateAgent(data);
        data.SetAgent(agent);
        data.SetAgentActivity(data.alive);

        if (index >= 0)
        {
            _agentControllers[index] = data;
        }

        activeAgents++;
        return index;
    }



    public void ChangeAgentType(AgentBehaviourData data)
    {
        data.SetAgent(AgentFactory.CreateAgent(data));
    }

    // Method returning by reference
    public ref AgentBehaviourData GetAgentControllerDataByRef(int index)
    {
        return ref _agentControllers[index];
    }

    // Method returning by value
    public AgentBehaviourData GetAgentControllerData(int index)
    {
        if (index >= 0 && index < maxAgents)
        {
            return _agentControllers[index];
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index out of range.");
        }
    }

    /* public void ReviveAgent(int index)
     {
         _agentControllers[index].alive = true;
         _agentControllers[index].col.isTrigger = true;
         _agentControllers[index].navMesh.enabled = true;
         _agentControllers[index].rb.useGravity = false;
     }*/

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
    private void LeftShiftArray()
    {
        if (activeAgents <= 0)
            return; // No need to shift if there are no active agents

        // Shift elements
        for (int i = 0; i < activeAgents - 1; i++)
        {
            _agentControllers[i] = _agentControllers[i + 1];
        }

        // Set the last element to default value of AgentControllerData
        _agentControllers[activeAgents - 1] = default(AgentBehaviourData);

        activeAgents--; // Decrement activeAgents count
    }

}
