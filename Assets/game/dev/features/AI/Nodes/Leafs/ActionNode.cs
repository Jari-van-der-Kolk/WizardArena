using System;
using UnityEngine;

namespace Saxon.BT
{
    public class ActionNode : LeafNode
    {
        private readonly Func<NodeState> action;

        public ActionNode(Func<NodeState> action)
        {
            this.action = action;
        }
        protected override NodeState OnUpdate()
        {
            return action();
        }

       
    }
}
