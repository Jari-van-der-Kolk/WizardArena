using System;

namespace Saxon.BT
{
    public class ControlNode : LeafNode
    {

        bool nextNode;
        public ControlNode(bool nextNode)
        {
            this.nextNode = nextNode;
        }

        protected override void OnStart() { }

        protected override NodeState OnUpdate()
        {
            if(nextNode)
            {
                return NodeState.Success;
            }
            else
            {
                return NodeState.Failure;
            }

        }

        internal override void OnStop() { }
    }
}
