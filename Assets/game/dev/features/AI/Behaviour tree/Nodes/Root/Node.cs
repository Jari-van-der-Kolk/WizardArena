using UnityEngine;
using UnityEngine.AI;

namespace Saxon.BT
{
    //Debug.Log(this.ToString() + " stopped " + state.ToString());

    public abstract class Node
    {
        public string debug;
        public INodeDebugger debugger;
        public Agent agent;

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

        protected virtual void OnStart() { }
        internal virtual void OnStop() { }
        protected abstract NodeState OnUpdate();

        public void Print(object message)
        {
            Debug.Log(message);
        }

    }
}