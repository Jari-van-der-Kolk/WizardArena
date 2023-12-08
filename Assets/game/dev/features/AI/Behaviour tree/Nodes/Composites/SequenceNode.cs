using System;
using System.Collections.Generic;
using UnityEngine;

namespace Saxon.BT
{
    public class SequenceNode : CompositeNode
    {
        bool debug;
        Action activeResponse;

        public SequenceNode(List<Node> children)
        {
            this.children = children;
        }  
        public SequenceNode(Action activeResponse, List<Node> children)
        {
            this.children = children;
            this.activeResponse = activeResponse;
        }
        public SequenceNode(List<Node> children, bool debug)
        {
            this.children = children;
            this.debug = debug;
        }

        
        protected override void OnStart()
        {
            index = 0;
        }

        internal override void OnStop()
        {
            index = 0;
        }

        protected override State OnUpdate()
        {
            while (index < children.Count)
            {
                var child = children[index].Update();

                if (child == State.Success) {
                    index++;
                }
                else if (child == State.Running ) {
                    activeResponse?.Invoke();
                    return child;
                }
                else if (child == State.Failure) {
                    return child;
                }
            }

            index = 0;
            return State.Success;

        }
    }
}
         



/*var child = children[index];


switch (child.Update())
{
    case State.Running:
        return State.Running;
    case State.Failure:
        return State.Failure;
    case State.Success:
        index++;
        break;
}
return index >= children.Count ? State.Success : State.Running;*/