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
    [SerializeField] private GameObject _agentControllerPrefab;

     public static GameObject agentControllerPrefab { get; private set; }

    //mutable
    private AgentControllerData[] _agentControllers = new AgentControllerData[maxAgents];

    private void Awake()
    {
        agentControllerPrefab = _agentControllerPrefab; 
    }

    private void Update()
    {
        for (int i = 0; i < _agentControllers.Length; i++)
        {
            if (_agentControllers[i].enabled)
            {
                _agentControllers[i].Poll(_BTUpdateStep, _detectionUpdateStep);
            }
        } 
    }
    public static void Spawn(Transform spawnLocation, AgentType agentType)
    {
        Instantiate(agentControllerPrefab, spawnLocation.position, Quaternion.identity);   
        
    }

    //this method usually gets called during compile time
    public void Register(AgentControllerData agentController)
    {
        int index = SearchForUnallocatedSpot();
        if(index >= 0)
        {
            _agentControllers[index] = agentController;
        }
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


