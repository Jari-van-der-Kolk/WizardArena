using System;
using System.Collections.Generic;
using UnityEngine;

namespace Saxon.BT
{
    public class NecroSpell : LeafNode
    {
        Collider[] colliders = new Collider[50];
        Agent agent;
        float radiusEffect;

        public NecroSpell( Agent agent, float radiusEffect)
        {
            this.agent = agent;
            this.radiusEffect = radiusEffect;
        }

        protected override void OnStart()
        {
            List<GameObject> revivableObjects = agent.detection.GetObjectsInVicinity();   
            for (int i = 0; i < revivableObjects.Count; i++)
            {

            }
        }

        protected override NodeState OnUpdate()
        {
            return NodeState.Success;
        }

        internal override void OnStop()
        {
            
        }
    }
}
