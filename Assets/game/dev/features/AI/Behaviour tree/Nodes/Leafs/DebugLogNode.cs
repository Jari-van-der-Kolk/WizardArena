using System;
using UnityEngine;

namespace Saxon.BT
{
    public class DebugLogNode<T> : LeafNode
    {

        private T message;

        public DebugLogNode(T message)
        {
            this.message = message;
        }
        
        protected override void OnStart()
        {
            Debug.Log($"OnStart: {message}");
        }

        internal override void OnStop()
        {
            Debug.Log($"OnStop: {message}");
        }

        protected override NodeState OnUpdate()
        {
            Debug.Log($"OnUpdate: {message}");
            return NodeState.Success;
        }
    }
}