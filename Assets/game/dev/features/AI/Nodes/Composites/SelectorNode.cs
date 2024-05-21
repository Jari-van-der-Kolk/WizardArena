using System.Collections.Generic;
using UnityEngine;

namespace Saxon.BT
{
    public class SelectorNode : CompositeNode, INodeDebugger
    {
        public SelectorNode(List<Node> children) : base(children) { }
        public SelectorNode(string name, List<Node> children) : base(children) 
        {
            debug = name;
            debugger = this;
        
        }

        public void Debugger<T>(T debug)
        {
            Debug.Log(this.ToString() + " stopped " + state.ToString());

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
            HaltChildren();
            return NodeState.Failure;
        }

       
    }
}
