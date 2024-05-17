using Saxon.BT.AI;
using Saxon.BT;
using System.Collections;
using UnityEngine;
using Saxon.BT.AI.Controller;
using DependencyInjection;

namespace Saxon.BT.AI.Types
{
    public enum AgentTypes
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

        public Agent Create(AgentTypes agentType, AgentController owner)
        {
            Agent agent = null;

            switch (agentType)
            {
                case AgentTypes.CloseRangeWizard:
                    agent = new CloseRangeWizard(owner);
                    break;
                case AgentTypes.MidRangeWizard:
                    agent = new MidRangeWizard(owner);
                    break;
                case AgentTypes.LongRangeWizard:
                    agent = new LongRangeWizard(owner);
                    break;
                case AgentTypes.Necromancer:
                    agent = new Necromancer(owner);
                    break;
                case AgentTypes.NecroServant:
                    agent = new NecroServant(owner);
                    break;
                case AgentTypes.Spider:
                    agent = new Spider(owner);
                    break;
                // Add more cases as needed
                default:
                    Debug.Log("You might want to assign the: " + agentType +
                        " Inside of the Factory method found inside of: " + gameObject);
                    break;
            }

            return agent;
        }
    }
}