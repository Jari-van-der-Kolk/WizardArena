using System;

namespace Saxon.BT
{
    public class RootNode : DecoratorNode
    {
        public RootNode(Node childNode)
        {
            child = childNode;
        }

        public float deltaTime { get; set; }

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

        public void SetDeltaTime(float deltaTime)
        {
            this.deltaTime = deltaTime;
        }


    }
}
