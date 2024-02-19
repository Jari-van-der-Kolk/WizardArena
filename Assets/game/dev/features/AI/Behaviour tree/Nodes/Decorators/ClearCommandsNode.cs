using UnityEngine;
namespace Saxon.BT
{
    public class ClearCommandsNode : DecoratorNode, INodeDebugger
    {
        CommandInvoker commandInvoker;
        

        public ClearCommandsNode(Node childNode, CommandInvoker commandInvoker)
        {
            child = childNode;
            this.commandInvoker = commandInvoker;

        }

        public ClearCommandsNode(string debug, Node childNode, CommandInvoker commandInvoker)
        {
            debugger = this;
            this.debug = debug;
            child = childNode;
            this.commandInvoker = commandInvoker;

        }

        public void Debugger<T>(T debug)
        {
            Debug.Log(base.debug + " " + state);

        }

        protected override void OnStart()
        {
            commandInvoker.ClearAllCommands();
        }

        protected override NodeState OnUpdate()
        {
            return child.Update();
        }
        
    }
}
