using UnityEngine;

namespace Saxon.BT
{
    public class WaitNode : LeafNode, INodeDebugger
    {
        public float duraction = 1;
        private float startTime;

        public WaitNode(float duraction)
        {
            this.duraction = duraction;
        }

        protected override void OnStart()
        {
            startTime = Time.time;
        }

        internal override void OnStop() { }

        protected override NodeState OnUpdate()
        {
            if (Time.time - startTime > duraction)
            {
                return NodeState.Success;
            }

            return NodeState.Running;
        }

        public void Debugger<T>(T debug)
        {
            
        }
    }
}