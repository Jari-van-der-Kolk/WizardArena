using Saxon.BT.AI.Controller;
using Saxon.BT.AI.Types;



namespace Saxon.BT.AI
{
    public class LongRangeWizard : Agent
    {
        public LongRangeWizard(AgentControllerData agentControllerData) : base(agentControllerData)
        {
        }

        public override AgentType agentType { get { return AgentType.CloseRangeWizard; } protected set { } }

        public override BehaviourTree CreateTree()
        {
            throw new System.NotImplementedException();
        }
    }
}