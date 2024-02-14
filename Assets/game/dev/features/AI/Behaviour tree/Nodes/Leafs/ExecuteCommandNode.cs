using System.Collections.Generic;
using UnityEngine;

namespace Saxon.BT
{
    public class ExecuteCommandNode : LeafNode, INodeDebugger
    {
        readonly bool queueCommand;

        readonly Command command;

        bool commandsExecuted;

        public ExecuteCommandNode(Agent agent,Command command, bool queueCommand = true)
        {
            this.agent = agent;
            this.command = command;
            this.queueCommand = queueCommand;
        }

        public void Debugger<T>(T debug)
        {
            Debug.Log(base.debug + " " + state);
        }

        protected override void OnStart()
        {
            if(command.isExecuted && !queueCommand)
            {
                agent.commandInvoker.CancelCommand(command);
                agent.commandInvoker.AddCommand(command);
            }
            else if(command.isExecuted)
            {
                agent.commandInvoker.queueCommands.AddCommand(command);
            }
        }

        protected override NodeState OnUpdate()
        {
            return command.isExecuted ? NodeState.Success : NodeState.Running;
        }

      


    }

}
