using System.Net.Mail;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Saxon.BT
{

    public class RootNode
    {
        private float startTime;

        public Node rootNode;
        public Node.State treeState = Node.State.Running;
        public AgentController agentController;

        public RootNode(Node rootNode)
        {
            startTime = Time.time;
            this.rootNode = rootNode;
        }  
        
        public void TimeStepUpdate(float timestep)
        {
            float time = Time.time;
            if (time - startTime > timestep)
            {
                treeState = rootNode.Update();
                startTime = time;
            }
        }
    }

}
            //if(treeState == Node.State.Running)