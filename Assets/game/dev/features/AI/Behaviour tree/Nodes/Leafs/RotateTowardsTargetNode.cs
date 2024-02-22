using Saxon.Sensor;
using System;
using UnityEngine;

namespace Saxon.BT
{
    public class RotateTowardsTargetNode : LeafNode
    {
        readonly float rotationSpeed;
        public RotateTowardsTargetNode(Agent agent, float rotationSpeed)
        {
            this.agent = agent;
            this.rotationSpeed = rotationSpeed;
        }

        protected override void OnStart()
        {
            base.OnStart();
            agent.navMesh.updateRotation = false;

        }

        internal override void OnStop()
        {
            base.OnStop();
            agent.navMesh.updateRotation = true;
        }

        protected override NodeState OnUpdate()
        {

            Vector3 direction = (agent.target.position - agent.position).normalized;
            if (direction == Vector3.zero)
            {
                return NodeState.Running;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, targetRotation, rotationSpeed * agent.rootNode.deltaTime);

            if (Quaternion.Angle(agent.transform.rotation, targetRotation) <= 0.1f)
            {
                return NodeState.Success;
            }

            return NodeState.Running;
        }

        
    }
}
