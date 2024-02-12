using System;
using UnityEngine;

namespace Saxon.BT
{
    public class SetDestinationNode : LeafNode
    {
        Transform destinationTransform;
        float reachedTargetDistance;
        bool inDistance;

        public SetDestinationNode(Agent agent, float reachedTargetDistance, Transform destinationTransform)
        {
            this.agent = agent;
            this.reachedTargetDistance = reachedTargetDistance;
            this.destinationTransform = destinationTransform;
        }

        protected override void OnStart()
        {
            inDistance = Saxon.IsInDistance(agent.position, destinationTransform.position, reachedTargetDistance);
            if(!inDistance)
            {
                agent.SetDestination(destinationTransform.position);
            }
            else
            {
                agent.SetDestination(agent.position);
            }
        }

        protected override NodeState OnUpdate()
        {
            return NodeState.Success;
        }

        internal override void OnStop()
        {
        }
    }
}
