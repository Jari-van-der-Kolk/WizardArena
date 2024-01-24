using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Saxon.BT
{
    public class ReactiveFallback : CompositeNode
    {

        public ReactiveFallback(List<Node> children)
        {
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
            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];

                switch (child.Update())
                {
                    case NodeState.Running:
                        return NodeState.Running;
                    case NodeState.Failure:
                        continue;
                    case NodeState.Success:
                        return NodeState.Running;
                    default:
                        break;
                }
            }

            return NodeState.Failure;
        }

    }

}