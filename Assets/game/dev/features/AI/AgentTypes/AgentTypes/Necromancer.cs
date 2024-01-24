using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Saxon.Sensor;
using Saxon.BT.AI.Controller;
using Saxon.NodePositioning;
using UnityEngine.UIElements;
using Saxon.BT;

namespace Saxon.BT.AI
{
    public class Necromancer : Agent
    {
        public Necromancer(AgentController agent) : base(agent) { }

        public override AgentTypes agentType { get { return AgentTypes.Necromancer; } protected set { } }

        public override BehaviourTree CreateTree()
        {
            Saxon.GetPlayerObject(out var playerObject);
            SetDestinationNode followPlayer = new SetDestinationNode(this, 5f, playerObject.transform);
            InverterNode invertFollowPlayer = new InverterNode(followPlayer);



            ConditionNode hasLostVisuals = new ConditionNode("lost target",() => detection.targetRecentlyLost);
            SequenceNode lostTarget = new SequenceNode(new List<Node>
            {
                hasLostVisuals, invertFollowPlayer
            });

            ConditionNode hasVisuals = new ConditionNode(() => detection.hasTargetInSight);
            SequenceNode foundTarget = new SequenceNode("found target" ,new List<Node> 
            {
                hasVisuals, followPlayer, //TODO add here the functionality to attack the target
            });

            float findNewLocationRadius = 10f;
            float pickLocationRadius = 3f;

            RandomPatrolNode patrol = new RandomPatrolNode(this, findNewLocationRadius, pickLocationRadius);
            FallbackNode fallback = new FallbackNode(new List<Node> 
            {
                lostTarget, foundTarget ,patrol
            });

            RootNode rootNode = new RootNode(fallback);



            return new BehaviourTree(rootNode);
        }
    }
}
/* RandomPatrolNode randomPatrol = new RandomPatrolNode(this, 5f, 3f, spatialHashGrid);
            SetDestinationNode followPlayer = new SetDestinationNode(this, playerObject.transform);
            TargetInSightNode targetInSight = new TargetInSightNode(agentController.objectDetection);
            InverterNode inverter = new InverterNode(targetInSight);

            DebugLogNode<Node.State> debugLogNode = new DebugLogNode<Node.State>(inverter.state);
            DebugLogNode<string> logNode = new DebugLogNode<string>("f");

            

            ReactiveSequenceNode patrol = new ReactiveSequenceNode(new List<Node>
            {
                inverter, logNode
            });

            DebugLogNode<Node.State> state = new DebugLogNode<Node.State>(patrol.state);*/
/* RandomPatrolNode randomPatrol = new RandomPatrolNode(this, 5f, 3f, spatialHashGrid);
            ControlNode targetInSight = new ControlNode(hasTargetInSight);
            FallbackStarNode isPatroling = new FallbackStarNode(new List<Node>
            {
                targetInSight, randomPatrol       
            }); 

            SetDestinationNode followPlayer = new SetDestinationNode(this ,playerObject.transform);
            NodeControl isPursuing = new NodeControl(followPlayer, false);
            


            FallbackNode fallbackNode = new FallbackNode(new List<Node>
            {
                isPatroling, isPursuing
            });

*/