using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Saxon.BT
{

    public class FallbackStarNode : CompositeNode
    {

        public FallbackStarNode(List<Node> children)
        {
            this.children = children;     
        } 
       
        protected override void OnStart()
        {
            index = 0;
        }

        internal override void OnStop() {}

        protected override NodeState OnUpdate()
        {
            var child = children[index].Update();
            switch (child)
            {
                case NodeState.Running:
                    return NodeState.Running;
                case NodeState.Failure:
                    index++;
                    break;
                case NodeState.Success:
                    return NodeState.Running;
                default:
                    break;
            }
            return index >= children.Count ? NodeState.Success : NodeState.Running;
        }
    }
}
