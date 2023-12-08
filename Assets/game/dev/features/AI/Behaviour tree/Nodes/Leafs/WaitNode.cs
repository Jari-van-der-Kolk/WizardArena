using UnityEngine;

namespace Saxon.BT
{
    public class WaitNode : LeafNode
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

        protected override State OnUpdate()
        {
            if (Time.time - startTime > duraction)
            {
                return State.Success;
            }

            return State.Running;
        }
    }
}