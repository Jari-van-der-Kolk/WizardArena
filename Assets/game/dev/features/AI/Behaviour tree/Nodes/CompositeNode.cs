using System.Collections.Generic;

namespace JBehaviourTree
{
    public abstract class CompositeNode : Node
    {
        protected List<Node> children = new List<Node>();
    }
}