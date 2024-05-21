using System;
using UnityEngine;

namespace Saxon.BT
{
    public class FunctionNode : LeafNode
    {
        public delegate Node.NodeState ActionNodeDelegate();
        private ActionNodeDelegate m_action;


        public FunctionNode(ActionNodeDelegate action)
        {
            m_action = action;
        }

        protected override void OnStart() { }

        internal override void OnStop() { }

        protected override NodeState OnUpdate()
        {
            switch (m_action()) 
            {
                case NodeState.Success:
                    state = NodeState.Success;
                    return state;
                case NodeState.Failure:
                    state = NodeState.Failure;
                    return state;
                case NodeState.Running:
                    state = NodeState.Running;
                    return state;
                default:
                    state = NodeState.Failure;
                    return state;
            }
        } 
    }
}
