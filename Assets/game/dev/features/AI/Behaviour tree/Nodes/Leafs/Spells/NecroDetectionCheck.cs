using System;
using UnityEngine;

namespace Saxon.BT
{
    public class NecroDetectionCheck : LeafNode
    {
        NecroSpell necroSpell;


        public NecroDetectionCheck(Agent agent, NecroSpell necroSpell)
        {
            this.agent = agent;
            this.necroSpell = necroSpell;
        }

       
        protected override NodeState OnUpdate()
        {
            return CheckServantsDetection();
        }

        private NodeState CheckServantsDetection()
        {
            NodeState state = NodeState.None;



            return NodeState.None;
        } 

        
    }
}
