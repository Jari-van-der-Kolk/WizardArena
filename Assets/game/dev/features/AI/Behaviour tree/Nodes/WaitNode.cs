using UnityEngine;

namespace JBehaviourTree
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
            Debug.Log("Wait for " + duraction);
            startTime = Time.time;
        }

        protected override void OnStop() { }

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