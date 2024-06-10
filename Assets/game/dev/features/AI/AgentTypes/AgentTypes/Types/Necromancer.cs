using System.Collections.Generic;
using UnityEngine;
using Saxon.BT.AI.Controller;
using Saxon.BT.AI.Types;

namespace Saxon.BT.AI
{
    public class Necromancer : Agent
    {
        public Necromancer(AgentControllerData agent) : base(agent) { }

        public override AgentType agentType { get { return AgentType.Necromancer; } protected set { } }

        NecroSpell necroSpell;

        public override BehaviourTree CreateTree()
        {
           
            int amountOfDeadNearby = 3;
            float necroReviveRadius = 5f;
            SetDestinationNode standStill = new SetDestinationNode(this, 3f, transform);
            ConditionNode enoughDeadAgents = new ConditionNode(() => CountDeadAgentsInVicinity(necroReviveRadius) > amountOfDeadNearby);
            necroSpell = new NecroSpell(this, necroReviveRadius);
            WaitNode cast = new WaitNode(3f);
            SequenceNode castNecroSpell = new SequenceNode(new Node[]
            {
                enoughDeadAgents, standStill, cast, necroSpell
            });

            float findNewLocationRadius = 10f;
            float pickLocationRadius = 3f;
            OriginPatrolNode patrol = new OriginPatrolNode(this, findNewLocationRadius, pickLocationRadius);

            ConditionNode servantsDetection = new ConditionNode(() => CheckServantsDetection(necroSpell.controllingAgents));
            SequenceNode servantsHaveSeenTarget = new SequenceNode("s see",new Node[]
            {
                servantsDetection, ChaseTarget(detection.data.longRangeAttackDistance)
            });

            SelectorNode chaseCheck = new SelectorNode(new Node[]
            {
                recentlyLostTarget, targetInSight
            });

            SequenceNode chaseTarget = new SequenceNode(new Node[]
            {
                chaseCheck, ChaseTarget(detection.data.longRangeAttackDistance)
            });

            SelectorNode fallback = new SelectorNode(new Node[]
            {
               castNecroSpell, servantsHaveSeenTarget, chaseTarget, patrol
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

        public bool CheckServantsDetection(List<AgentController> agents)
        {
            if (agents.Count > 0)
            {
                for (int i = 0; i < agents.Count; i++)
                {
                    /*if (agents[i].objectDetection.hasTargetInSight)
                    {
                        var target = agents[i].objectDetection.target;
                        detection.SetTarget(target);

                        for (int c = 0; c < agents.Count; c++)
                        {
                            agents[c].objectDetection.ToggleTargetRecentlyLost(true);
                            agents[c].objectDetection.ResetRecentlyLostTimer();
                            agents[c].objectDetection.SetTarget(target);
                        }
                        
                        return true;
                    }*/

                }
            }
            return false;
        }




        #endregion
    }
}
