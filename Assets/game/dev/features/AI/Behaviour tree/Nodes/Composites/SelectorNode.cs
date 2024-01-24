using System;
using System.Collections.Generic;

namespace Saxon.BT
{
    public class SelectorNode : CompositeNode
    {
        public SelectorNode(List<Node> children)
        {
            this.children = children;
        }

        protected override void OnStart()
        {
        }

        protected override NodeState OnUpdate()
        {
            foreach (var child in children)
            {
                NodeState childState = child.Update();

                // If any child succeeds, the Selector returns success
                if (childState == NodeState.Success)
                {
                    return NodeState.Success;
                }

                // If a child is still running, the Selector returns running
                if (childState == NodeState.Running)
                {
                    return NodeState.Running;
                }
            }

            // If all children fail, the Selector returns failure
            return NodeState.Failure;
        }

        internal override void OnStop()
        {
        }
    }
}
