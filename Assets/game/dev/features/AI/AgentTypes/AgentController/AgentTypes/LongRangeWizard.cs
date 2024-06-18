using Saxon.BT.AI.Controller;
using Saxon.BT.AI.Types;



namespace Saxon.BT.AI
{
    public class LongRangeWizard : Agent
    {
        public override RootNode rootNode { get; protected set; }
        public LongRangeWizard(AgentBehaviourData agentData) : base(agentData)
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