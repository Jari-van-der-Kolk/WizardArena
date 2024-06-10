using System.Collections.Generic;
using Saxon.BT.AI.Controller;
using Saxon.BT.AI.Types;


namespace Saxon.BT.AI
{
    internal class Spider : Agent
    {
        public Spider(AgentControllerData agentControllerData) : base(agentControllerData) { }

        public override AgentType agentType { get { return AgentType.CloseRangeWizard; } protected set { } }


        public override BehaviourTree CreateTree()
        {

            FallbackNode fallback = new FallbackNode(new Node[]
            {

            });

            RootNode root = new RootNode(fallback);

            return new BehaviourTree(root);
        }
    }
}