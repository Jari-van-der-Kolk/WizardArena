using UnityEngine;

namespace JBehaviourTree
{
    public class FunctionNode : LeafNode
    {
        public delegate Node.State ActionNodeDelegate();
        private ActionNodeDelegate m_action;

        public FunctionNode(ActionNodeDelegate action) {
            m_action = action;
        }

        protected override void OnStart()
        {
        }

        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            switch (m_action()) {
                case Node.State.Success:
                    state = Node.State.Success;
                    return state;
                case Node.State.Failure:
                    state = Node.State.Failure;
                    return state;
                case Node.State.Running:
                    state = State.Running;
                    return state;
                default:
                    state = Node.State.Failure;
                    return state;
            }
        } 
    }
}
