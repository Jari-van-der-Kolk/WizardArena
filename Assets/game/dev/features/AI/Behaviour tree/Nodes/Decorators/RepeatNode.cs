namespace Saxon.BT
{
    public class RepeatNode : DecoratorNode
    {
        private int index;
        private int repeatCount;
        bool keepRepeating;

        public RepeatNode(Node child)
        {
            this.child = child;
            keepRepeating = true;
        }

        public RepeatNode(Node child, int repeatCount)
        {
            this.child = child;
            this.repeatCount = repeatCount;
            keepRepeating = false;
        }
        
        protected override void OnStart() 
        {
            index = 0;
        }
        internal override void OnStop() { }
        protected override State OnUpdate()
        {
            child.Update();


            switch (keepRepeating)
            {
                case true:
                    return State.Running;
                case false:
                    index++;
                    break;
            }

            return index >= repeatCount ? State.Success : State.Running;
        }
    }
}