using System.Net.Mail;
using JBehaviourTree;

    public class RootNode
    {
        public Node rootNode;
        public Node.State treeState = Node.State.Running;

        public RootNode(Node rootNode)
        {
            this.rootNode = rootNode;
        }
        
        public void Update()
        {
            if(rootNode.state == Node.State.Running)
                treeState = rootNode.Update();
        }
    }
