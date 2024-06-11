using System.Collections;
using System.Collections.Generic;
using Saxon.BT.AI.Types;
using Saxon.BT.AI.Controller;


namespace Saxon.BT.AI
{
    public class MidRangeWizard : Agent
    {
        public override RootNode rootNode { get; protected set; }
        public MidRangeWizard(AgentControllerData agentData) : base(agentData)
        {
            rootNode = CreateTree();
        }



        public override AgentType agentType { get { return AgentType.CloseRangeWizard; } protected set { } }

        public override RootNode CreateTree()
        {
            throw new System.NotImplementedException();
        }
    }
}