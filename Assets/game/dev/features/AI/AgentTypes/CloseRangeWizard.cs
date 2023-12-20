using Saxon;
using Saxon.BT;
using Saxon.NodePositioning;
using Saxon.Sensor;
using UnityEngine.AI;

namespace Saxon.BT.AI
{
    internal class CloseRangeWizard : Agent
    {
        public CloseRangeWizard(AgentController agentController) : base(agentController) { }

        public override AgentTypes agentType { get { return AgentTypes.CloseRangeWizard; } protected set { } }

        public override RootNode CreateTree()
        {
            Saxon.GetPlayer(out var playerObject);
            RandomPatrol patrol = new RandomPatrol(this, 10f, 3f, NodeGenerator.Instance.hashGrid);
            NodeControl control = new NodeControl(patrol, playerObject);
            return new RootNode(control);
        }
    }
}