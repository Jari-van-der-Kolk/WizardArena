using DependencyInjection;
using Saxon.BT.AI.Controller;
using Saxon.BT.AI.Types;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour, IDependencyProvider
{
    [Provide]
    public AgentManager ProvideAgentManager() 
    {
        return this;
    }
    //config
    const int maxAgents = 1000;

    [Header("Agent update rate")]
    [Tooltip("The speed at which the behaviour tree updates")]
    [SerializeField] private float _BTUpdateStep = 0.5f;
    [Tooltip("The speed at which the object detection updates")]
    [SerializeField] private float _detectionUpdateStep = 0.5f;

    //mutable
    private AgentController[] _agentControllers = new AgentController[maxAgents];


    private void Update()
    {
        for (int i = 0; i < _agentControllers.Length; i++)
        {
            if(_agentControllers[i] != null)
            {
                _agentControllers[i].Poll(_BTUpdateStep, _detectionUpdateStep);
            }

        } 
    }

    //this method usually gets called during compile time
    public void Register(AgentController agentController)
    {
        int index = SearchForUnallocatedSpot();
        if(index >= 0)
        {
            agentController.Init();
            _agentControllers[index] = agentController;
        }
    }

    public void Spawn(Transform spawnLocation, AgentType agentType)
    {
        int index = SearchForUnallocatedSpot();
        if (index >= 0)
        {

        }
        
    }

    // Private Methods
    public int SearchForUnallocatedSpot()
    {
        for (int i = 0; i < _agentControllers.Length; i++)
        {
            if (_agentControllers[i] == null)
            {
                return i;
            }
        }

        Debug.LogWarning("No unallocated spot available for spawning new agent.");
        return -1; // Indicate no spot available
    }
    
}


