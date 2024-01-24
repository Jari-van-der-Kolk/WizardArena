using System;
using UnityEngine;

namespace Saxon.BT
{
    public class SetTargetNode : LeafNode
    {
        Transform destinationTransform;
        Agent agent;
        GameObject player;

        public SetTargetNode(Agent agent,Transform destinationTransform)
        {
            this.agent = agent;
            this.destinationTransform = destinationTransform;
            
            Saxon.GetPlayerObject(out player);

        }

        protected override void OnStart()
        {
            agent.SetDestination(destinationTransform.position);            
        }

        protected override NodeState OnUpdate()
        {
            if (Saxon.IsInDistance(agent.position, destinationTransform.position, 3f) && agent.detection.HasObjectInSight(player)){
                return NodeState.Success;
            }

            return NodeState.Running;
        }

        internal override void OnStop()
        {
            
        }
    }
}
