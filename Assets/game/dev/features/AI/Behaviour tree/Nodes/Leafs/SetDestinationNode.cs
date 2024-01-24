using System;
using UnityEngine;

namespace Saxon.BT
{
    public class SetDestinationNode : LeafNode
    {
        Transform destinationTransform;
        Agent agent;
        float reachedTargetDistance;

        public SetDestinationNode(Agent agent, float reachedTargetDistance, Transform destinationTransform)
        {
            this.agent = agent;
            this.reachedTargetDistance = reachedTargetDistance;
            this.destinationTransform = destinationTransform;
        }

        protected override void OnStart()
        {
            if(!Saxon.IsInDistance(agent.position, destinationTransform.position, reachedTargetDistance))
                agent.SetDestination(destinationTransform.position);  
        }

        protected override NodeState OnUpdate()
        {
            if (Saxon.IsInDistance(agent.position, destinationTransform.position, reachedTargetDistance))
            {
                return NodeState.Success;
            }

            return NodeState.Failure;
        }

        internal override void OnStop()
        {
            agent.SetDestination(agent.position);
        }
    }
}
