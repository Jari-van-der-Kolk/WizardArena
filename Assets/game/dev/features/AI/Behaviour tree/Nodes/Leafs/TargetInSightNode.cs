using Saxon.Sensor;
using System;

namespace Saxon.BT
{
    public class TargetInSightNode : LeafNode
    {

        ObjectDetection objectDetection;
        public TargetInSightNode(ObjectDetection detectionData)
        {
            this.objectDetection = detectionData;
        }

        protected override void OnStart()
        {

        }

        protected override NodeState OnUpdate()
        {
            if (objectDetection.hasTargetInSight)
            {
                return NodeState.Success;
            }
            else
            {
                return NodeState.Failure;
            }
        }

        internal override void OnStop()
        {
            
        }
    }
}
