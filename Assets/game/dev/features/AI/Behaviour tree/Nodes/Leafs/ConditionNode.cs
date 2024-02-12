using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Saxon.BT
{

    public class ConditionNode : DecoratorNode, INodeDebugger
    {
        private Func<bool> conditionFunction;
        public ConditionNode(Func<bool> condition)
        {
            conditionFunction = condition;
        }
        public ConditionNode(Node child)
        {
            this.child = child;
        }

        public ConditionNode(string name, Func<bool> condition)
        {
            //gets handles in the Node class
            debugger = this;
            conditionFunction = condition;
            this.debug = name;
        }

        protected override void OnStart() 
        {

        }

        internal override void OnStop() { 

        }

        protected override NodeState OnUpdate()
        {
            if (conditionFunction())
            {
                return NodeState.Success; // Condition is true
            }
            if (child?.Update() == NodeState.Success)
            {
                return NodeState.Success; // Condition is true
            }

            return NodeState.Failure; // Condition is false
        }

        public void Debugger<T>(T debug)
        {
            Debug.Log(base.debug + " " + state + " " + conditionFunction());    
        }
    }

}