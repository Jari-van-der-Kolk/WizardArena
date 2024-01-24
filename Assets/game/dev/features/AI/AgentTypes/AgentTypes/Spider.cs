using Saxon;
using Saxon.BT;
using Saxon.NodePositioning;
using Saxon.Sensor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Saxon.BT.AI.Controller;

namespace Saxon.BT.AI
{
    internal class Spider : Agent
    {
        public Spider(AgentController agentController) : base(agentController) { }

        public override AgentTypes agentType { get { return AgentTypes.CloseRangeWizard; } protected set { } }


        public override BehaviourTree CreateTree()
        {

            FallbackNode fallback = new FallbackNode(new List<Node>
            {

            });

            RootNode root = new RootNode(fallback);

            return new BehaviourTree(root);
        }
    }
}