using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Saxon.Sensor;
using Saxon.BT.AI.Controller;
using Saxon.NodePositioning;
using UnityEngine.UIElements;
using Saxon.BT;
using System;

namespace Saxon.BT.AI
{
    public class Necromancer : Agent
    {
        public Necromancer(AgentController agent) : base(agent) { }

        public override AgentTypes agentType { get { return AgentTypes.Necromancer; } protected set { } }

        public override BehaviourTree CreateTree()
        {
            MoveToDestinationCommand standStillCommand = new MoveToDestinationCommand(navMesh, detection );
            ExecuteCommandNode standStill = new ExecuteCommandNode(this, standStillCommand);
            int amountOfDeadNearby = 3;
            float necroReviveRadius = 5f;
            ConditionNode enoughDeadAgents = new ConditionNode(() => CountDeadAgentsInVicinity(necroReviveRadius) > amountOfDeadNearby);
            NecroSpell necroSpell = new NecroSpell(this, necroReviveRadius);
            WaitNode cast = new WaitNode(3f);
            SequenceNode castNecroSpell = new SequenceNode("spell", new List<Node>
            {
                enoughDeadAgents, standStill, cast, necroSpell
            });

            float findNewLocationRadius = 10f;
            float pickLocationRadius = 3f;
            RandomPatrolNode patrol = new RandomPatrolNode("patrol", this, findNewLocationRadius, pickLocationRadius);


            RootNode rootNode = new RootNode(ChasePlayer(4f));

            return new BehaviourTree(rootNode);
        }
        public int CountDeadAgentsInVicinity(float searchRadius)
        {
            int count = 0;
            var agentsInVicinity = detection.GetComponentsInArea<AgentController>(searchRadius);                  
                 
            if( agentsInVicinity != null )
            {
                for (int i = 0; i < agentsInVicinity.Count; i++)
                {
                    if (!agentsInVicinity[i].alive && agentsInVicinity[i].transform != agentController.transform)
                    {
                        count++;
                    }
                }
            }
            else
            {
                Debug.Log("you're trying to get a object that does not contain the correct Component, switch layer or add the component");
                return 0;
            }
          

            return count;
        }


        public int CountDeadAgentsInVicinity(float searchRadius, out List<AgentController> agents)
        {
            int count = 0;
            agents = new List<AgentController>();
            var agentsInVicinity = detection.GetComponentsInArea<AgentController>(searchRadius);
            for (int i = 0; i < agentsInVicinity.Count; i++)
            {
                if (!agentsInVicinity[i].alive && agentsInVicinity[i].transform != agentController.transform)
                {
                    agents.Add(agentsInVicinity[i]);    
                    count++;
                }
            }

            return count;
        }
    }
}
    /* Saxon.GetPlayerObject(out var playerObject);
            RotateTowardsCommand rotationCommand = new RotateTowardsCommand(transform, detection, 2f);
            FollowTargetNode followTarget = new FollowTargetNode(this, 5f, rotationCommand);
            SetDestinationNode targetPlayer = new SetDestinationNode(this, 5f, playerObject.transform);


            ConditionNode targetSightCondition = new ConditionNode(() => detection.targetRecentlyLost || detection.hasTargetInSight);
            ReturnStateNode pauseNode = new ReturnStateNode(Node.NodeState.Success);
            SequenceNode chaseTarget = new SequenceNode("chase",new List<Node>                                      
            {                                                    
                targetSightCondition, followTarget
            });

           
           
          
           

       */

    /*  ConditionNode lostTargetCondition = new ConditionNode(() => detection.targetRecentlyLost);
            SequenceNode lostTarget = new SequenceNode(new List<Node>
            {
                lostTargetCondition, followTarget
            });


            ConditionNode foundTargetCondition = new ConditionNode(() => detection.hasTargetInSight);

            SequenceNode foundTarget = new SequenceNode(new List<Node> 
            {
                foundTargetCondition, followTarget, pauseNode
            });*/