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

        protected override State OnUpdate()
        {
            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Failure:
                    return State.Success;
                case State.Success:
                    return State.Failure;
                default:
                    break;
            }
            return State.Success;
        }

      
    }

}