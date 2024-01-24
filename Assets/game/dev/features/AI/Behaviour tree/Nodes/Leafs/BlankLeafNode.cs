using System;

namespace Saxon.BT
{
    public class BlankLeafNode : LeafNode
    {
        public BlankLeafNode() 
        {

        }

        protected override void OnStart()
        {
            throw new NotImplementedException();
        }

        protected override NodeState OnUpdate()
        {
            throw new NotImplementedException();
        }

        internal override void OnStop()
        {
            throw new NotImplementedException();
        }
    }
}
