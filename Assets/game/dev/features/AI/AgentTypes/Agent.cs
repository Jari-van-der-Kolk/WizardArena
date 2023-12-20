using Saxon.BT;
using Saxon.Sensor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Saxon.BT
{
    public enum AgentTypes
    {
        CloseRangeWizard,
        MidRangeWizard,
        LongRangeWizard
    };

    public abstract class Agent
    {
        public abstract AgentTypes agentType { get; protected set; }
    
        public AgentController agentController;
        public NavMeshAgent navMesh;
        public ObjectDetection detection;
    
        public Agent(AgentController agentController)
        {
            this.agentController = agentController;
            navMesh = agentController.navMesh;
            detection = agentController.objectDetection;
        }

        public abstract RootNode CreateTree();
    }

}