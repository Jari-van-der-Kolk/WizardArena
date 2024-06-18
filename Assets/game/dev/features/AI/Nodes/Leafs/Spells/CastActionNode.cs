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
        readonly AgentBehaviourData data;
        ActionType actionType;

        public CastActionNode(AgentBehaviourData agent, ActionType action)
        {
            data = agent;
            actionType = action;
        }

        protected override void OnStart()
        {
            ActionManager.CastSpellByReference(data.monoBehaviour, data.actionRegister.GetRandomActionFromRegister(actionType), data.targetTag);       
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
