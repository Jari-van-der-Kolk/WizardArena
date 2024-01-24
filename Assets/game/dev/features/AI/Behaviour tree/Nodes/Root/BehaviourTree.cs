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

        public Node rootNode;
        public Node.NodeState treeState = Node.NodeState.Running;
        public AgentController agentController;

        public BehaviourTree(Node rootNode)
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