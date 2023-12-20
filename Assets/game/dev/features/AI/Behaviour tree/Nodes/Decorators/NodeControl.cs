using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Saxon.BT
{

    public class NodeControl : DecoratorNode
    {
        public bool continueNode;
        
        public NodeControl(Node child, bool continueNode)
        {
            this.child = child;
            this.continueNode = continueNode;
        }

        protected override void OnStart() { }

        internal override void OnStop() { }

        protected override State OnUpdate()
        {
            State state = child.Update();
            
            if (continueNode == true) {
                return State.Success;
            }
            
            child.Update();
            return state;
            
        }
    }

}