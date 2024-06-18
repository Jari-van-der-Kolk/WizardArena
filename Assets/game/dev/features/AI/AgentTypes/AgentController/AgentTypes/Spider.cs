using System.Collections.Generic;
using Saxon.BT.AI.Controller;
using Saxon.BT.AI.Types;


namespace Saxon.BT.AI
{
    internal class Spider : Agent
    {
        public override RootNode rootNode { get; protected set; }

        public Spider(AgentBehaviourData agentControllerData) : base(agentControllerData) { rootNode = CreateTree(); }

        public override AgentType agentType { get { return AgentType.CloseRangeWizard; } protected set { } }


        public override RootNode CreateTree()
        {

            FallbackNode fallback = new FallbackNode(new Node[]
            {

            });

            RootNode root = new RootNode(fallback);

            return root;
        }
    }
}