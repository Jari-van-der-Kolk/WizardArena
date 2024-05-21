using UnityEngine;

namespace Saxon.BT
{
    public class TimedRepeatNode : DecoratorNode
    {
        public float duraction = 1;
        private float startTime;

        public TimedRepeatNode(Node child, float duraction)
        {
            this.child = child;
            this.duraction = duraction;
        }

        protected override void OnStart()
        {
            startTime = Time.time;
            Debug.Log(this.ToString() + " started");
        }

        internal override void OnStop() {
            Debug.Log(this.ToString() + " stopped");
        }

        protected override NodeState OnUpdate()
        {
            if (Time.time - startTime > duraction)
            {
                return NodeState.Success;
            }

            child.Update();
            return NodeState.Running;
        }
    }
}