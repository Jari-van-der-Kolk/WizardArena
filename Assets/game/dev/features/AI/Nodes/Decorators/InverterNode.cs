using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Saxon.BT
{

    public class InverterNode : DecoratorNode
    {

        public InverterNode(Node child)
        {
            this.child = child;
        }

        protected override void OnStart() { }

        internal override void OnStop() { }

        protected override NodeState OnUpdate()
        {
            switch (child.Update())
            {
                case NodeState.Running:
                    return NodeState.Running;
                case NodeState.Failure:
                    return NodeState.Success;
                case NodeState.Success:
                    return NodeState.Failure;
                default:
                    break;
            }
            return NodeState.Success;
        }

      
    }

}