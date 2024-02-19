using System;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

namespace Saxon.BT
{
    public class SequenceNode : CompositeNode, INodeDebugger
    {

        CommandInvoker commandInvoker;
        Command command;
        public SequenceNode(List<Node> children)
        {
            this.children = children;
        }  
        public SequenceNode(CommandInvoker commandInvoker, List<Node> children)
        {
            this.children = children;
            this.commandInvoker = commandInvoker;
        }

        public SequenceNode(CommandInvoker commandInvoker,  Command command, List<Node> children)
        {
            this.children = children;
            this.commandInvoker = commandInvoker;
            this.command = command;
        }
        public SequenceNode(string name, List<Node> children)
        {
            this.debug = name;
            this.debugger = this;
            this.children = children;
        }

        protected override void OnStart()
        {
            commandInvoker?.ClearAllCommands();
            if (command != null)
            {
                commandInvoker.AddCommand(command);
            }
        }

        internal override void OnStop()
        {

        }

        protected override NodeState OnUpdate()
        {

            foreach (var child in children)
            {
                var childStatus = child.Update();

                if(childStatus == NodeState.Failure)
                {
                    return NodeState.Failure;
                }

                if(childStatus == NodeState.Running)
                {
                    return NodeState.Running;

                }
            }
             
            
            return NodeState.Success;

        }

        public void Debugger<T>(T debug)
        {
            Debug.Log(base.debug + " " + state);
        }
    }
}

/*while (index < children.Count)
{
    var child = children[index].Update();

    if (child == State.Success)
    {
        index++;
    }
    else if (child == State.Running)
    {
        activeResponse?.Invoke();
        return child;
    }
    else if (child == State.Failure)
    {
        return child;
    }
}

index = 0;
*/


/*var child = children[index];


switch (child.Update())
{
    case State.Running:
        return State.Running;
    case State.Failure:
        return State.Failure;
    case State.Success:
        index++;
        break;
}
return index >= children.Count ? State.Success : State.Running;*/