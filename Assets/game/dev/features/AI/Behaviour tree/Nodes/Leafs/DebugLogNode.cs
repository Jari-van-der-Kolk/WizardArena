using UnityEngine;

namespace Saxon.BT
{
    public class DebugLogNode : LeafNode
    {

        private string message;

        public DebugLogNode(string message)
        {
            this.message = message;
        }
        
        protected override void OnStart()
        {
            Debug.Log($"OnStart {message}");
        }

        internal override void OnStop()
        {
            Debug.Log($"OnStop {message}");
        }

        protected override State OnUpdate()
        {
            Debug.Log($"OnUpdate {message}");
            return State.Success;
        }
    }
}