using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Saxon.BT
{

    public class RandomDelayNode : LeafNode
    {
        private float min;
        private float max;
        private float startTime;
        private float duraction;

        public RandomDelayNode(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        protected override void OnStart()
        {
            duraction = Random.Range(min, max);
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
    }

}