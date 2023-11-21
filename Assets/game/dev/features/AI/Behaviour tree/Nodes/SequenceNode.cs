using System.Collections.Generic;
using UnityEngine;

namespace JBehaviourTree
{
    public class SequenceNode : CompositeNode
    {
        private int current;

        public SequenceNode(List<Node> children)
        {
            this.children = children;
        }
        
        protected override void OnStart()
        {
            current = 0;
        }

        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            var child = children[current];
            
            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Failure:
                    return State.Failure;
                case State.Success:
                    current++;
                    break;
            }
            return current >= children.Count ? State.Success : State.Running;
        }
    }
}