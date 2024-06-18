using Saxon.BT.AI;
using Saxon.BT;
using System.Collections;
using UnityEngine;
using Saxon.BT.AI.Controller;
using DependencyInjection;
using Saxon.BT.AI.Types;

namespace Saxon.BT.AI.Types
{
    public enum AgentType
    {
        CloseRangeWizard,
        MidRangeWizard,
        LongRangeWizard,
        Necromancer,
        NecroServant,
        Spider,
    };

    public class AgentFactory : MonoBehaviour, IDependencyProvider
    {
        [Provide]
        public AgentFactory ProvideAgentFactory()
        {
            return this;
        }

        public static Agent CreateAgent(AgentBehaviourData owner)
        {
            Agent agent = null;

            switch (owner.agentType)
            {
                case AgentType.CloseRangeWizard:
                    agent = new CloseRangeWizard(owner);
                    break;
                case AgentType.MidRangeWizard:
                    agent = new MidRangeWizard(owner);
                    break;
                case AgentType.LongRangeWizard:
                    agent = new LongRangeWizard(owner);
                    break;
                case AgentType.Necromancer:
                    agent = new Necromancer(owner);
                    break;
                case AgentType.NecroServant:
                    agent = new NecroServant(owner);
                    break;
                case AgentType.Spider:
                    agent = new Spider(owner);
                    break;
                // Add more cases as needed
                default:
                    Debug.Log("You might want to assign: " + owner.agentType + "inside of the AgentFactory");
                    break;
            }


            return agent;
        }


        
    }
}


    
        /*public AgentDirector director = new AgentDirector();


        public class AgentDirector
        {
            public AgentBuidler builder = new AgentBuidler();   

            public AgentControllerData Construct(AgentControllerData agentData)
            {
                builder.SetAgentType(agentData.agentType);
                
                return builder.Build();
            }
        }

        public class AgentBuidler
        {
            AgentControllerData agentData = new AgentControllerData();
            public AgentType agentType;
            public void SetAgentType(AgentType agentType)
            {
                agentData.agentType = agentType;  
            }
            
           
            public AgentControllerData Build()
            {
                return agentData; 
            }
           

        }*/