using System.Net.Mail;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Saxon.BT.AI.Controller;


namespace Saxon.BT
{

    public class BehaviourTree
    {
        private float startTime;

        public RootNode rootNode;
        public AgentController agentController;

        public BehaviourTree(RootNode rootNode)
        {
            startTime = Time.time;
            this.rootNode = rootNode;
        }  
        
        public void TimeStepUpdate(float timestep)
        {
            float time = Time.time;
            if (time - startTime > timestep)
            {
                rootNode.Update();
                startTime = time;
            }
        }
    }

}
