namespace JBehaviourTree
{
    public class RepeatNode : DecoratorNode
    {
        public RepeatNode(Node child)
        {
            this.child = child;
        }
        
        protected override void OnStart() { }
        protected override void OnStop(){ }
        protected override State OnUpdate()
        {
            child.Update();
            return State.Running;
        }
    }
}