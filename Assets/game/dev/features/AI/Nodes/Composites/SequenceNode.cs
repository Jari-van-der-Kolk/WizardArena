using System;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

namespace Saxon.BT
{
    public class SequenceNode : CompositeNode, INodeDebugger
    {
        public SequenceNode(List<Node> children) : base(children) { }  
      
        public SequenceNode(string name, List<Node> children) : base(children)
        {
            debug = name;
            debugger = this;
        }

        protected override NodeState OnUpdate()
        {
            foreach (var child in children)
            {
                var childStatus = child.Update();

                if(childStatus == NodeState.Failure)
                {
                    HaltChildren();
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