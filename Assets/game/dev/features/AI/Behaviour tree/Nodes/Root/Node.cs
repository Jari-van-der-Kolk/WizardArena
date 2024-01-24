using UnityEngine;
using UnityEngine.AI;

namespace Saxon.BT
{
    //Debug.Log(this.ToString() + " stopped " + state.ToString());

    public abstract class Node
    {
        public string name;
        public INodeDebugger debugger;

        public enum NodeState
        {
            Running,
            Failure,
            Success
        }

        public NodeState state = NodeState.Running;
        public bool started = false;

        public NodeState Update()
        {
            if (!started)
            {
                OnStart();
                started = true;
            }

            state = OnUpdate();

            if (state == NodeState.Failure || state == NodeState.Success)    
            {
                OnStop();
                started = false;
                debugger?.Debugger(this);
            }

            return state;
        }

        protected abstract void OnStart();
        internal abstract void OnStop();
        protected abstract NodeState OnUpdate();

    }
}