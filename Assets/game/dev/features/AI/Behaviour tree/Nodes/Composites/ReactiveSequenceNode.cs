using System.Collections.Generic;
using UnityEngine;

namespace Saxon.BT
{
    public class ReactiveSequenceNode : CompositeNode
    {
        public ReactiveSequenceNode(List<Node> children)
        {
            this.children = children;
        }
        
        protected override void OnStart() { }

        internal override void OnStop() { }

        protected override State OnUpdate()
        {
            bool anyChildRunning = false;
            
            foreach (Node node in children)
            {
                switch (node.Update())
                {
                    case State.Running:
                        anyChildRunning = true;
                        continue;
                    case State.Failure:
                        HaltChildren();
                        return State.Failure;
                    case State.Success:
                        continue;
                    default:
                        return State.Success;
                }
            }
            return anyChildRunning ? State.Running : State.Success; 
        }
    }
}