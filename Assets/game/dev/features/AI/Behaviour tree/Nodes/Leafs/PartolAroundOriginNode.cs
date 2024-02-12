using Saxon.HashGrid;
using System;

namespace Saxon.BT
{
    public class PartolAroundOriginNode : LeafNode
    {

        public PartolAroundOriginNode(Agent agent)
        {
            this.agent = agent;
        }

        protected override void OnStart()
        {
            
        }

        protected override NodeState OnUpdate()
        {
            throw new NotImplementedException();
        }

        internal override void OnStop()
        {
            
        }
    }
}
