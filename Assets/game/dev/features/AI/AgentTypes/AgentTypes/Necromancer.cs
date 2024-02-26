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
           
            int amountOfDeadNearby = 3;
            float necroReviveRadius = 5f;
            SetDestinationNode standStill = new SetDestinationNode(this, 3f, transform);
            ConditionNode enoughDeadAgents = new ConditionNode(() => CountDeadAgentsInVicinity(necroReviveRadius) > amountOfDeadNearby);
            NecroSpell necroSpell = new NecroSpell(this, necroReviveRadius);
            WaitNode cast = new WaitNode(3f);
            SequenceNode castNecroSpell = new SequenceNode(new List<Node>
            {
                enoughDeadAgents, standStill, cast, necroSpell
            });

            float findNewLocationRadius = 10f;
            float pickLocationRadius = 3f;
            OriginPatrolNode patrol = new OriginPatrolNode(this, findNewLocationRadius, pickLocationRadius);

            SequenceNode lostPlayer = new SequenceNode(new List<Node>
            {
                recentlyLostTarget, patrol
            });

            SelectorNode fallback = new SelectorNode(new List<Node>
            {
               castNecroSpell, ChaseTarget(detection.data.midRangeAttackDistance), patrol
            });

            rootNode = new RootNode(fallback);

            return new BehaviourTree(rootNode);
        }

        #region functions
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

        #endregion
    }
}
