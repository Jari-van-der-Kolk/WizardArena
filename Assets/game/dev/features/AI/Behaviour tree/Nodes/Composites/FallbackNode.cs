using System.Collections.Generic;
using UnityEngine;

namespace Saxon.BT
{
    public class FallbackNode : CompositeNode
    {

        public FallbackNode(List<Node> children)
        {
            this.children = children;
        }

        protected override void OnStart() 
        {
            index = 0;
        }

        internal override void OnStop() { }

        protected override State OnUpdate()
        {
            while (index < children.Count)
            {
                var child = children[index].Update();

                if (child == State.Running) {
                    return State.Running;
                }
                else if (child == State.Failure) {
                    index++;
                }
                else if (child == State.Success) {
                    index = 0;
                    return State.Running;
                }

            }

            index = 0;
            return State.Failure;

            /*foreach (Node node in children)
            {
                switch (node.Update())
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
            return State.Failure;*/
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