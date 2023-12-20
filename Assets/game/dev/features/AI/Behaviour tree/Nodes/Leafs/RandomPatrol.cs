using Saxon.HashGrid;
using Saxon.NodePositioning;
using System.Collections.Generic;
using UnityEngine;

namespace Saxon.BT
{
    public class RandomPatrol : LeafNode
    {
        Agent agent;
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
        SpatialHashGrid<HashNode> hashGrid;
        Vector3 destination;

        float searchRadius;
        float searchStartLength;
        int dirIndex;

        public RandomPatrol(Agent agent, float searchStartLength ,float searchRadius ,SpatialHashGrid<HashNode> spatialHashGrid)
        {
            this.agent = agent;
            this.hashGrid = spatialHashGrid;
            this.searchRadius = searchRadius;
            this.searchStartLength = searchStartLength;
        }


        protected override void OnStart()
        {
            int maxIndex = directions.Length;        
            dirIndex = Random.Range(0, maxIndex);
            destination = GetLocation();
            agent.navMesh.SetDestination(destination);
        }

        protected override State OnUpdate()
        {
           
            if(Saxon.IsInDistance(agent.navMesh.transform.position, destination, 1f))
            {
                return State.Success;
            }
            else
            {
                return State.Running;
            }
        }

        internal override void OnStop() {}


        public Vector3 GetLocation()
        {
            Vector3 loc = Vector3.zero;
            Vector3 dir = directions[dirIndex];
            Vector3 pos = agent.navMesh.transform.position + (dir * searchStartLength);       

            List<HashNode> result = hashGrid.GetItemsInRadius(pos, searchRadius);
            int randomResult = Random.Range(0, result.Count);
            HashNode gridNode = result[randomResult];

            if(gridNode == null)
            {
                GetLocation();
            }
            else
            {
                loc = gridNode.transform.position; 
            }
            return loc;
        }
    }
}
