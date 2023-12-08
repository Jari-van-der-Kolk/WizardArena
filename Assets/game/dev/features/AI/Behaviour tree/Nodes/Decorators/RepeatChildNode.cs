using UnityEngine;

namespace Saxon.BT
{
    public class RepeatChildNode : DecoratorNode
    {
        public float duraction = 1;
        private float startTime;

        public RepeatChildNode(Node child, float duraction)
        {
            this.child = child;
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

            child.Update();
            return State.Running;
        }
    }
}