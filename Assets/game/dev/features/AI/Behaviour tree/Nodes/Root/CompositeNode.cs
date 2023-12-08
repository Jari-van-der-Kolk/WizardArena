using System.Collections.Generic;

namespace Saxon.BT
{
    public abstract class CompositeNode : Node
    {
        public bool hasResponse;
        protected List<Node> children = new List<Node>();
        public int index;

        internal void HaltChildren()
        {
            for (int i = 0; i < children.Count; i++)
            {
                children[i].OnStop();
            }
        }
    }
}