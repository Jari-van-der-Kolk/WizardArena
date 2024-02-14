using Saxon.BT;
using Saxon.Sensor;
using UnityEngine;
using UnityEngine.AI;

public class MoveAgentCommand : Command
{
    private readonly Agent agent;
    private readonly NavMeshAgent navMeshAgent;
    private readonly float stoppingDistance;


    public MoveAgentCommand(NavMeshAgent navMeshAgent, Agent agent)
    {
        this.navMeshAgent = navMeshAgent;
        this.agent = agent;
        this.stoppingDistance = navMeshAgent.stoppingDistance;

        // Ensure the GameObject has a NavMeshAgent component
        if (navMeshAgent == null)
        {
            Debug.LogError("MoveToDestinationCommand requires a NavMeshAgent component on the GameObject.");
        }

    }
    public MoveAgentCommand(NavMeshAgent navMeshAgent, Agent agent, float stoppingDistance)
    {
        this.navMeshAgent = navMeshAgent;
        this.agent = agent;
        this.stoppingDistance = stoppingDistance;

        // Ensure the GameObject has a NavMeshAgent component
        if (navMeshAgent == null)
        {
            Debug.LogError("MoveToDestinationCommand requires a NavMeshAgent component on the GameObject.");
        }

    }

    public override bool MoveNext()
    {
        if (isExecuted)
        {
            return false;
        }

        bool inRange = navMeshAgent.remainingDistance <= stoppingDistance;
       
        if (inRange)
        {
            navMeshAgent.SetDestination(agent.target.position);
        }  
        else if (!navMeshAgent.pathPending && inRange)
        {
            isExecuted = true;
        }
       

        return !isExecuted;
    }

    public override void Reset()
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.ResetPath();
        }

    }

    public override object Current => null;
}

