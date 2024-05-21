using Saxon.HashGrid;
using Saxon.NodePositioning;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Saxon.BT
{
    public class OriginPatrolNode : LeafNode, INodeDebugger
    {
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
        Vector3 destination;

        float searchRadius;
        float searchStartLength;

        private float successDelay = 2.0f; // Adjust the delay time as needed
        private float startTime;

        public OriginPatrolNode(Agent agent, float searchStartLength ,float searchRadius)
        {
            this.agent = agent;
            this.searchRadius = searchRadius;
            this.searchStartLength = searchStartLength;
        }

        public OriginPatrolNode(string name, Agent agent, float searchStartLength, float searchRadius)
        {
            debugger = this; 
            this.debug = name;
            this.agent = agent;
            this.searchRadius = searchRadius;
            this.searchStartLength = searchStartLength;
        }

        protected override void OnStart()
        {
            agent.navMesh.updateRotation = true;
            SetNewDestination();
            startTime = Time.time; // Record the start time
        }

        protected override NodeState OnUpdate()
        {
            if(agent.agentController.destination != destination)
            {
                SetNewDestination();
            }

            if (Saxon.IsInDistance(agent.navMesh.transform.position, destination, 2.5f))
            {
                // Check if the required delay has passed
                if (Time.time - startTime >= successDelay)
                {
                    return NodeState.Success;
                }
                else
                {
                    return NodeState.Running;
                }
            }
            else
            {
                // Reset the start time when the agent is moving
                startTime = Time.time;
                return NodeState.Running;
            }
        }
    
        private void SetNewDestination()
        {
            destination = GetLocation();
            agent.SetDestination(destination);
            agent.navMesh.SetDestination(destination);
            if (!IsPathReachable(destination))
            {
                SetNewDestination();
            }
        }
        bool IsPathReachable(Vector3 targetPosition)
        {
            NavMeshPath path = new NavMeshPath();

            // Calculate the path
            if (NavMesh.CalculatePath(agent.position, targetPosition, NavMesh.AllAreas, path))
            {
                // Check if the path status is not PathInvalid
                if (path.status != NavMeshPathStatus.PathInvalid)
                {
                    return true;
                }
            }

            return false;
        }

        public Vector3 GetLocation()
        {
            int dirIndex = Random.Range(0, directions.Length);
            Vector3 dir = directions[dirIndex];
            Vector3 pos = agent.origin.position + (dir * searchStartLength);

            List<HashNode> result = agent.spatialHashGrid.GetItemsInRadius(pos, searchRadius);

            Vector3 loc = Vector3.zero;
            if (result.Count > 0)
            {
                int randomResult = Random.Range(0, result.Count);
                HashNode gridNode = result[randomResult];

                if (gridNode != null)
                {
                    loc = gridNode.transform.position;
                }
            }

            return loc;
        }

        public List<Vector3> CheckOpenSurroundings()
        {
            List<Vector3> openCornorsDirections = new List<Vector3>();
            float checkDistance = 20f;
            float[] distanceValues = new float[directions.Length];
            for (int i = 0; i < directions.Length; i++)
            {
                Vector3 pos = agent.position;
                Ray ray = new Ray(pos, directions[i]);
                Physics.Raycast(ray, out var result, checkDistance);
                distanceValues[i] = Vector3.Distance(pos, result.point);
            }

            int maxIndex = 0;
            float maxValue = distanceValues[0];

            for (int i = 1; i < distanceValues.Length; i++)
            {
                if (distanceValues[i] > maxValue)
                {
                    maxValue = distanceValues[i];
                    maxIndex = i;
                }
                if (distanceValues[i] >= checkDistance)
                {
                    openCornorsDirections.Add(directions[i]);
                }
            }

            openCornorsDirections.Add(directions[maxIndex]);



            return openCornorsDirections;
        }

        public void Debugger<T>(T debug)
        {
            Debug.Log(base.debug + " " + state);
        }
    }
}

