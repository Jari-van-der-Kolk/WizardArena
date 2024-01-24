using System;

namespace Saxon.BT
{
    public class RootNode : DecoratorNode
    {

        public RootNode(Node childNode)
        {
            child = childNode;
        }

        protected override void OnStart()
        {
        }

        protected override NodeState OnUpdate()
        {
            var childStatus = child.Update();

            switch (childStatus)
            {
                case NodeState.Success:
                    return NodeState.Success;

                case NodeState.Failure:
                    return NodeState.Failure;

                case NodeState.Running:
                default:
                    return NodeState.Running;
            }

        }

        internal override void OnStop()
        {
        }
    }
}
