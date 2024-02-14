using Saxon.BT;
using Saxon.Sensor;
using UnityEngine;
using UnityEngine.AI;

public class GetTargetInSightCommand : Command
{
    private readonly ObjectDetection detection;
    private readonly NavMeshAgent navMeshAgent;
    private readonly float stoppingDistance;
    bool inRange;



    public GetTargetInSightCommand(NavMeshAgent navMeshAgent, ObjectDetection detection)
    {
        this.navMeshAgent = navMeshAgent;
        this.detection = detection;
        this.stoppingDistance = navMeshAgent.stoppingDistance;
    }
   

    public override void Enlisted()
    {
        base.Enlisted();
        inRange = Vector3.Distance(navMeshAgent.transform.position, detection.target.position) <= stoppingDistance;
        Move();

    }

    public override bool MoveNext()
    {
        if (isExecuted)
        {
            return false;
        }

        inRange = navMeshAgent.remainingDistance <= stoppingDistance;
        Move();

        if (!navMeshAgent.pathPending && inRange)
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

    public void Move()
    {
        if (inRange)
        {
            navMeshAgent.SetDestination(navMeshAgent.transform.position);
        }
        else
        {
            navMeshAgent.SetDestination(detection.target.position);
        }

    }

    public override object Current => null;
}

