using System.Collections.Generic;
using UnityEngine;

namespace Saxon.BT
{
    public class ExecuteCommandNode : LeafNode
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
      
        protected override void OnStart()
        {
            if(command.isExecuted && !queueCommand)
            {
                agent.commandInvoker.CancelCommand(command);
                agent.commandInvoker.AddCommand(command);
            }
            else if(command.isExecuted)
            {
                Debug.Log("enlist");

                agent.commandInvoker.queueCommands.AddCommand(command);
            }
        }

        protected override NodeState OnUpdate()
        {
            return command.isExecuted ? NodeState.Success : NodeState.Running;
        }

      


    }

}
