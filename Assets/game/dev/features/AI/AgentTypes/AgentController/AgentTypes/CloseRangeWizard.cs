using Saxon.BT.AI.Types;
using Saxon.BT.AI.Controller;

namespace Saxon.BT.AI
{
    internal class CloseRangeWizard : Agent
    {
        public CloseRangeWizard(AgentControllerData agentData) : base(agentData)
        {
            CreateTree();
        }

        public override AgentType agentType { get { return AgentType.CloseRangeWizard; } protected set { } }

        public override RootNode rootNode { get; protected set; }

        public override RootNode CreateTree()
        {

            SetDestinationNode destinationNode = new SetDestinationNode(this, 3f,transform);

            RepeatNode repeatNode = new RepeatNode(destinationNode, 4);


            OriginPatrolNode patrol = new OriginPatrolNode(this, 10f, 3f);
            //NodeControl control = new NodeControl(waypointPatrolNode, playerObject);

            RootNode rootNode = new RootNode(destinationNode);

            return new RootNode(rootNode);
        }
    }
}