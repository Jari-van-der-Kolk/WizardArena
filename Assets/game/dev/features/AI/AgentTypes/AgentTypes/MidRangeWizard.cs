using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Saxon.Sensor;
using Saxon.BT.AI.Controller;


namespace Saxon.BT.AI
{
    public class MidRangeWizard : Agent
    {
        public MidRangeWizard(AgentController agent) : base(agent)
        {
        }

        public override AgentTypes agentType { get { return AgentTypes.CloseRangeWizard; } protected set { } }

        public override BehaviourTree CreateTree()
        {
            throw new System.NotImplementedException();
        }
    }
}