using Saxon.BT;
using Saxon.Sensor;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Saxon.BT.AI
{
    public class LongRangeWizard : Agent
    {
        public LongRangeWizard(AgentController agent) : base(agent)
        {
        }

        public override AgentTypes agentType { get => throw new System.NotImplementedException(); protected set => throw new System.NotImplementedException(); }

        public override RootNode CreateTree()
        {
            throw new System.NotImplementedException();
        }
    }
}