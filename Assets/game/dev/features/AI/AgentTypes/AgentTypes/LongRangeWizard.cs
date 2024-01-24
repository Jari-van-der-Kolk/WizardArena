using Saxon.BT;
using Saxon.Sensor;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Saxon.BT.AI.Controller;


namespace Saxon.BT.AI
{
    public class LongRangeWizard : Agent
    {
        public LongRangeWizard(AgentController agent) : base(agent)
        {
        }

        public override AgentTypes agentType { get { return AgentTypes.CloseRangeWizard; } protected set { } }

        public override BehaviourTree CreateTree()
        {
            throw new System.NotImplementedException();
        }
    }
}