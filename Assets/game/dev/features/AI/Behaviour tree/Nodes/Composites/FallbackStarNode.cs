using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Saxon.BT
{

    public class FallbackStarNode : CompositeNode
    {

        bool debug;
        public FallbackStarNode(List<Node> children)
        {
            this.children = children;     
        } 
       
        protected override void OnStart()
        {
            index = 0;
        }

        internal override void OnStop() {}

        protected override State OnUpdate()
        {
            var child = children[index].Update();
            switch (child)
            {
                case State.Running:
                    return State.Running;
                case State.Failure:
                    index++;
                    break;
                case State.Success:
                    return State.Running;
                default:
                    break;
            }
            return index >= children.Count ? State.Success : State.Running;
        }
    }
}
