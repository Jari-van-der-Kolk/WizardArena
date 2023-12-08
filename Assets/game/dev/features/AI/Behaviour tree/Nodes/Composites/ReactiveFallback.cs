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

        protected override State OnUpdate()
        {
            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];

                switch (child.Update())
                {
                    case State.Running:
                        return State.Running;
                    case State.Failure:
                        continue;
                    case State.Success:
                        return State.Running;
                    default:
                        break;
                }
            }

            return State.Failure;
        }

    }

}