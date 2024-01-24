using System.Collections.Generic;
using UnityEngine;

namespace Saxon.BT
{
    public class FallbackNode : CompositeNode, INodeDebugger
    {


        public FallbackNode(List<Node> children)
        {
            this.children = children;
        }

        public FallbackNode(string name, List<Node> children)
        {
            this.name = name;
            this.debugger = this;
            this.children = children;
        }

        protected override void OnStart() 
        {

        }

        internal override void OnStop() 
        {

        }

        protected override NodeState OnUpdate()
        {
            
            foreach (Node node in children)
            {
                var childStatus = node.Update();
                if (childStatus == NodeState.Success || childStatus == NodeState.Running)
                {
                    return childStatus;
                }

            }
            return NodeState.Failure;

        }

        public void Debugger<T>(T debug)
        {
            Debug.Log(name + " " + state);
        }
    }
}

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