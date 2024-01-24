namespace Saxon.BT
{
    public class RepeatNode : DecoratorNode
    {
        private int index;
        private int repeatCount;

        public RepeatNode(Node child)
        {
            this.child = child;
        }

        public RepeatNode(Node child, int repeatCount)
        {
            this.child = child;
            this.repeatCount = repeatCount;
        }
        
        protected override void OnStart() 
        {
            index = 0;
        }
        protected override NodeState OnUpdate()
        {
            child.Update();

            if(child.state == NodeState.Success)
            {
                index++;
                if (index > repeatCount)
                {
                    return NodeState.Success;
                }
            }
            else if(child.state == NodeState.Failure)
            {
                return NodeState.Failure;
            }
                

            return NodeState.Running;
           
            
        }

        internal override void OnStop() { }

    }
}
            //return index >= repeatCount ? State.Success : State.Running;