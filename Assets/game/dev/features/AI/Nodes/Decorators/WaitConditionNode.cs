using System;
using UnityEngine;

namespace Saxon.BT
{
    public class WaitConditionNode : DecoratorNode
    {
        public float duraction = 1;
        private float startTime;

        public WaitConditionNode(Node child, float duraction)
        {
            this.child = child;
            this.duraction = duraction; 
        }

        protected override void OnStart()
        {
            startTime = Time.time;
        }

        protected override NodeState OnUpdate()
        {
            var childStatus = child.Update();

            if(childStatus == NodeState.Failure)
            {
                return NodeState.Failure;
            }
            
            if (Time.time - startTime > duraction)
            {
                return NodeState.Success;
            }

            return NodeState.Running;


        }

        internal override void OnStop()
        {
        }
    }
}
