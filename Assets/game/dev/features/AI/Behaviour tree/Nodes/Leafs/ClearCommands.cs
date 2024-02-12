using System;

namespace Saxon.BT
{
    public class ClearCommands : LeafNode
    {
        public ClearCommands(Agent agent)
        {
            this.agent = agent;
        }
        protected override void OnStart()
        {
           agent.commandInvoker.queueCommands.ClearQueueCommands();
        }

        protected override NodeState OnUpdate()
        {
            return NodeState.Success;
        }

        internal override void OnStop()
        {
            
        }
    }
}
