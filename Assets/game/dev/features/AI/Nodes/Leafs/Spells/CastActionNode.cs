using Saxon.BT.AI;
using Saxon.BT.AI.Controller;
using System;
using System.Collections.Generic;
using UnityEngine;
using Saxon.BT.AI.Types;

namespace Saxon.BT
{
    public class CastActionNode : LeafNode, INodeDebugger
    {
        new readonly AgentControllerData agent;


        public CastActionNode(AgentControllerData agent, ActionType action)
        {
            this.agent = agent;
        }

        protected override void OnStart()
        {
        
        }

        protected override NodeState OnUpdate()
        {
            return NodeState.Success;
        }

        internal override void OnStop()
        {

        }
        public void Debugger<T>(T debug)
        {
            Debug.Log(base.debug + " " + state);
        }


    }
}
