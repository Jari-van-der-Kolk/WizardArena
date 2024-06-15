using System.Collections.Generic;
using UnityEngine;
using Saxon.BT.AI.Controller;
using Saxon.BT.AI.Types;

namespace Saxon.BT.AI
{
    public class Necromancer : Agent
    {
        public override RootNode rootNode { get; protected set; }

        public Necromancer(AgentControllerData agentData) : base(agentData) { rootNode = CreateTree(); }

        public override AgentType agentType { get { return AgentType.Necromancer; } protected set { } }


        public override RootNode CreateTree()
        {
           
            int amountOfDeadNearby = 3;
            float necroReviveRadius = 5f;
            SetDestinationNode standStill = new SetDestinationNode(this, 3f, transform);
            ConditionNode enoughDeadAgents = new ConditionNode(() => CountDeadAgentsInVicinity(necroReviveRadius) > amountOfDeadNearby);
            var necroSpell = new CastActionNode(agentData, ActionType.Revive);
            WaitNode cast = new WaitNode(3f);
            SequenceNode castNecroSpell = new SequenceNode(new Node[]
            {
                enoughDeadAgents, standStill, cast, necroSpell
            });

            float findNewLocationRadius = 10f;
            float pickLocationRadius = 3f;
            OriginPatrolNode patrol = new OriginPatrolNode(this, findNewLocationRadius, pickLocationRadius);

            ConditionNode servantsDetection = new ConditionNode(() => CheckServantsDetection(agentData.controllingAgents));
            SequenceNode servantsHaveSeenTarget = new SequenceNode("s see",new Node[]
            {
                servantsDetection, ChaseTarget(detection.Data.longRangeAttackDistance)
            });

            SelectorNode chaseCheck = new SelectorNode(new Node[]
            {
                recentlyLostTarget, targetInSight
            });

            SequenceNode chaseTarget = new SequenceNode(new Node[]
            {
                chaseCheck, ChaseTarget(detection.Data.longRangeAttackDistance)
            });

            SelectorNode fallback = new SelectorNode(new Node[]
            {
               castNecroSpell, servantsHaveSeenTarget, chaseTarget, patrol
            });

            rootNode = new RootNode(fallback);

            return rootNode;
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
                    if (!agentsInVicinity[i].IsAlive() && agentsInVicinity[i].transform != agentData.transform)
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
                if (agentsInVicinity[i].IsAlive() == false && agentsInVicinity[i].transform != agentData.transform)
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
