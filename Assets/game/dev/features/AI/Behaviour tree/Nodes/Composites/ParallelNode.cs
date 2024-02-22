using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Saxon.BT
{
    public class ParallelNode : CompositeNode
    {
        public ParallelNode(List<Node> children) : base(children) { }

        protected override NodeState OnUpdate()
        {
            bool anyChildRunning = false;
            bool allChildrenSucceeded = true;

            foreach (var child in children)
            {
                switch (child.Update())
                {
                    case NodeState.Running:
                        anyChildRunning = true;
                        break;
                    case NodeState.Failure:
                        allChildrenSucceeded = false;
                        break;
                }
            }

            if (allChildrenSucceeded)
            {
                return NodeState.Success;
            }
            else if (anyChildRunning)
            {
                return NodeState.Running;
            }
            else
            {
                return NodeState.Failure;
            }

        }

    }

}