using Saxon.Sensor;
using UnityEngine;
using UnityEngine.AI;

public class MoveToDestinationCommand : Command
{
    private readonly ObjectDetection detection;
    private readonly NavMeshAgent navMeshAgent;
    private readonly float stoppingDistance;


    public MoveToDestinationCommand(NavMeshAgent navMeshAgent, ObjectDetection detection)
    {
        this.navMeshAgent = navMeshAgent;
        this.detection = detection;
        this.stoppingDistance = navMeshAgent.stoppingDistance;

        // Ensure the GameObject has a NavMeshAgent component
        if (navMeshAgent == null)
        {
            Debug.LogError("MoveToDestinationCommand requires a NavMeshAgent component on the GameObject.");
        }

    }
    public MoveToDestinationCommand(NavMeshAgent navMeshAgent, ObjectDetection detection, float stoppingDistance)
    {
        this.navMeshAgent = navMeshAgent;
        this.detection = detection;
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


        // Set the destination and start moving   
        navMeshAgent.SetDestination(detection.target.position);

        // Mark the command as executed when the object reaches the destination
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= stoppingDistance)
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

