using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Saxon.BT
{

    //TODO rework this so that there is a backup leafnode
    public class FollowTargetNode : LeafNode
    {
        float  reachedTargetDistance;
        bool inDistance;
        bool hasOcclusion;
        bool targetRecentlyLost;

        NodeState nodeState;
        Command command;

        public FollowTargetNode(Agent agent, float reachedTargetDistance, Command command)
        {
            this.agent = agent;
            this.reachedTargetDistance = reachedTargetDistance;
            this.command = command;
        }

        protected override void OnStart()
        {
           
        }

        protected override NodeState OnUpdate()
        {
            hasOcclusion = agent.detection.HasOcclusionWithTarget();
            inDistance = agent.IsWithinDistanceCheck(reachedTargetDistance);
            targetRecentlyLost = agent.detection.targetRecentlyLost;


            if (!inDistance)
            {
                agent.SetDestination(agent.target.position);
                TargetIsOutOfSightCheck();
                nodeState = NodeState.Running;
            }
            else
            {
                TargetIsOutOfSightCheck();

                if (hasOcclusion)
                {
                    agent.SetDestination(agent.target.position);
                    nodeState = NodeState.Running;
                }
                else
                {
                    agent.commandInvoker.queueCommands.CancelCommand(command);
                    agent.SetDestination(agent.position);
                    nodeState = NodeState.Success;
                }

                
            }
           
            return nodeState;
        }

        internal override void OnStop()
        {
            if(state != NodeState.Running)
            {
                agent.navMesh.updateRotation = true;
            }
        }

        private void TargetIsOutOfSightCheck()
        {
            if (targetRecentlyLost)
            {
                agent.navMesh.updateRotation = false;
                agent.agentController.commandInvoker.queueCommands.AddCommand(command);
            }
            if (command.isExecuted)
            {
                agent.navMesh.updateRotation = true;
            }
        }
    }
}


/*inDistance = Saxon.IsInDistance(agent.position, agent.target.position, reachedTargetDistance);
if(rotate && !inDistance)
{
    agent.navMesh.updateRotation = false;
    agent.SetDestination(agent.target.position);
    Vector3 direction = agent.target.position - agent.position;
    if (direction != Vector3.zero)
    {
        //agent.agentController.commandInvoker.AddCommand()
    }
}
else if (!inDistance)
{
    agent.SetDestination(agent.target.position);
}
else
{
    agent.SetDestination(agent.position);
}
*/