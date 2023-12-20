using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saxon.BT
{
    
    public interface IAgentType
    {
        public AgentTypes AIType {  get; } 
        public RootNode CreateTree();
        public RootNode rootNode {  get; }

    }
}
