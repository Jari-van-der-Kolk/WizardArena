using System;

namespace Saxon.BT
{
    public class ReturnStateNode : LeafNode
    {
        NodeState nodeReturnState;

        public ReturnStateNode(NodeState nodeState)
        {
            nodeReturnState = nodeState;
        }
        protected override void OnStart()
        {
        }
        protected override NodeState OnUpdate()
        {
            return nodeReturnState;
        }
        internal override void OnStop()
        {
        }
    }
}
