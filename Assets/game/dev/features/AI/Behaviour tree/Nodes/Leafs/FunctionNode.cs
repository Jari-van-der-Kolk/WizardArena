using System;
using UnityEngine;

namespace Saxon.BT
{
    public class FunctionNode : LeafNode
    {
        public delegate Node.State ActionNodeDelegate();
        private ActionNodeDelegate m_action;


        public FunctionNode(ActionNodeDelegate action)
        {
            m_action = action;
        }

        protected override void OnStart() { }

        internal override void OnStop() { }

        protected override State OnUpdate()
        {
            switch (m_action()) 
            {
                case State.Success:
                    state = State.Success;
                    return state;
                case State.Failure:
                    state = State.Failure;
                    return state;
                case State.Running:
                    state = State.Running;
                    return state;
                default:
                    state = State.Failure;
                    return state;
            }
        } 
    }
}
