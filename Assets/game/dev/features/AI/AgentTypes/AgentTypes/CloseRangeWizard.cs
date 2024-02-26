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
    internal class CloseRangeWizard : Agent
    {
        public CloseRangeWizard(AgentController agentController) : base(agentController) { }

        public override AgentTypes agentType { get { return AgentTypes.CloseRangeWizard; } protected set { } }


        public override BehaviourTree CreateTree()
        {

            SetDestinationNode destinationNode = new SetDestinationNode(this, 3f,transform);

            RepeatNode repeatNode = new RepeatNode(destinationNode, 4);

            WaypointPatrolNode waypointPatrolNode = new WaypointPatrolNode(this);

            OriginPatrolNode patrol = new OriginPatrolNode(this, 10f, 3f);
            //NodeControl control = new NodeControl(waypointPatrolNode, playerObject);

            RootNode rootNode = new RootNode(destinationNode);

            return new BehaviourTree(rootNode);
        }
    }
}