﻿using Saxon.BT.AI;
using Saxon.BT.AI.Controller;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Saxon.BT
{
    public class NecroSpell : LeafNode, INodeDebugger
    {
        new readonly Necromancer agent;
        float radiusEffect;
        public List<AgentController> controllingAgents = new List<AgentController>();
       
        public NecroSpell(Necromancer agent, float radius)
        {
            this.agent = agent;
            this.radiusEffect = radius;
        }

        public NecroSpell(string name, Necromancer agent, float radius)
        {
            debugger = this;
            this.debug = name;
            this.agent = agent;
            this.radiusEffect = radius;
        }
       
        protected override void OnStart()
        {
            agent.CountDeadAgentsInVicinity(radiusEffect ,out var deadAgents);
            for (int i = 0; i < deadAgents.Count; i++)
            {
                deadAgents[i].SetAgentActivity(true);
                deadAgents[i].SetAgentType(AI.AgentTypes.NecroServant);
                deadAgents[i].SetOrigin(agent.transform);
                deadAgents[i].objectDetection = agent.detection;
                controllingAgents.Add(deadAgents[i]);
            }

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