using Saxon.BT;
using Saxon.Sensor;
using UnityEngine;
using UnityEngine.AI;

public class MoveToTargetCommand : Command
{
    private readonly ObjectDetection detection;
    private readonly NavMeshAgent navMeshAgent;
    private readonly float stoppingDistance;
    bool inRange;



    public MoveToTargetCommand(NavMeshAgent navMeshAgent, ObjectDetection detection)
    {
        this.navMeshAgent = navMeshAgent;
        this.detection = detection;
        this.stoppingDistance = navMeshAgent.stoppingDistance;
    }
    public MoveToTargetCommand(NavMeshAgent navMeshAgent, ObjectDetection detection, float stoppingDistance)
    {
        this.navMeshAgent = navMeshAgent;
        this.detection = detection;
        this.stoppingDistance = stoppingDistance;
    }

  
    public override bool MoveNext()
    {
        if (isExecuted)
        {
            return false;
        }

        Move();
        

        if (!navMeshAgent.pathPending && inRange && !detection.HasOcclusionWithTarget())
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
        inRange = Vector3.Distance(navMeshAgent.transform.position, detection.target.position) <= stoppingDistance;
        if (!inRange || detection.HasOcclusionWithTarget())
        {
            navMeshAgent.SetDestination(detection.target.position);
        }
        else
        {
            navMeshAgent.SetDestination(navMeshAgent.transform.position);
        }

    }

    public override object Current => null;
}

