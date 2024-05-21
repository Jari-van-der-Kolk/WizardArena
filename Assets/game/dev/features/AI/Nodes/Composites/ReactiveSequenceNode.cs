using System.Collections.Generic;
using UnityEngine;

namespace Saxon.BT
{
    public class ReactiveSequenceNode : CompositeNode
    {
        public ReactiveSequenceNode(List<Node> children) : base(children) { }
       
        protected override NodeState OnUpdate()
        {

            while (index < children.Count)
            {
                var child = children[index].Update();

                if (child == NodeState.Success)
                {
                    index++;
                }
                else if (child == NodeState.Running)
                {
                    return child;
                }
                else if (child == NodeState.Failure)
                {
                    index = 0;
                    return child;
                }
            }

            index = 0;
            return NodeState.Success;

        }
    }
}