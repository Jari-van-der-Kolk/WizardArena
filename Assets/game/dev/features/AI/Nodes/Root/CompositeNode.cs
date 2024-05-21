using System.Collections.Generic;

namespace Saxon.BT
{
    public abstract class CompositeNode : Node
    {
        protected readonly List<Node> children = new List<Node>();
        public int index;

        public CompositeNode(List<Node> children)
        {
            this.children = children;
        }

        internal void HaltChildren()
        {
            for (int i = 0; i < children.Count; i++)
            {
                children[i].OnStop();
                children[i].started = false;
            }
        }
    }
}