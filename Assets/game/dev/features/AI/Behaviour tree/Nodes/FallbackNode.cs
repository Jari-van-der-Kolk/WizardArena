using System.Collections.Generic;
using UnityEngine;

namespace JBehaviourTree
{
    public class FallbackNode : CompositeNode
    {
        private int current;

        public FallbackNode(List<Node> children)
        {
            this.children = children;
        }

        protected override void OnStart() { }

        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            foreach (var child in children)
            {
                switch (child.Update())
                {
                    case State.Failure:
                        continue;
                    case State.Success:
                        return State.Success;
                    case State.Running:
                        return State.Running;
                    default:
                        continue;
                }
            }
            return State.Failure;
        }
    }
}

/*public override NodeStates Evaluate() {
    foreach (Node node in m_nodes) {
        switch (node.Evaluate()) {
            case NodeStates.FAILURE:
                continue;
            case NodeStates.SUCCESS:
                m_nodeState = NodeStates.SUCCESS;
                return m_nodeState;
            case NodeStates.RUNNING:
                m_nodeState = NodeStates.RUNNING;
                return m_nodeState;
            default:
                continue;
        }
    }
    m_nodeState = NodeStates.FAILURE;
    return m_nodeState;
}*/