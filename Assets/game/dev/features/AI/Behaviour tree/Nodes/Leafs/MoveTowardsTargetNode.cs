using Saxon.Sensor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;

namespace Saxon.BT
{
    public class MoveTowardsTargetNode : LeafNode
    {

        readonly float stoppingDistance;

        public MoveTowardsTargetNode(Agent agent, float stoppingDistance)
        {
            this.stoppingDistance = stoppingDistance;
            this.agent = agent;
        }


        protected override NodeState OnUpdate()
        {
            bool outOfRange = Vector3.Distance(agent.navMesh.transform.position, agent.detection.target.position) >= stoppingDistance;
            if (outOfRange || agent.detection.HasOcclusionWithTarget())
            {
                agent.navMesh.SetDestination(agent.detection.target.position);
                return NodeState.Running;
            }
            else
            {
                agent.navMesh.SetDestination(agent.transform.position);
                return NodeState.Success;
            }
        }

        internal override void OnStop()
        {
            base.OnStop();
            if (agent.navMesh != null)
            {
                agent.navMesh.ResetPath();
            }
        }

    }
}

/*public class MoveToTargetCommand : Command
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

*/