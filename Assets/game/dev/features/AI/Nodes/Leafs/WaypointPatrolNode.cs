using Saxon.NodePositioning;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Saxon.BT
{
    public class WaypointPatrolNode : LeafNode
    {

        Vector3 destination;
        int index;
        public WaypointPatrolNode(Agent agent)
        {
            this.agent = agent;
        }

        protected override void OnStart()
        {
            if (index >= agent.agentController.waypoints.Count)
            {
                index = 0;
            }
            else
            {
                destination = agent.agentController.waypoints[index].position;
                agent.navMesh.SetDestination(destination);
            }

          
        }

        protected override NodeState OnUpdate()
        {
            if(Saxon.IsInDistance(agent.agentController.transform.position, destination, 2f)) { 
                index++;
                return NodeState.Success;
            }
            else
            { 
                return NodeState.Running; 
            }

        }

        internal override void OnStop()
        { 

        }
    }
}
