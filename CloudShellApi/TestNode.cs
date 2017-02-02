using System.Collections.Generic;

namespace QS.ALM.CloudShellApi
{
    public class TestNode
    {
        public string Name { get; private set; }
        public List<TestNode> Children { get; private set; }

        public TestNode(string name)
        {
            Name = name;
            Children = new List<TestNode>();
        }
    }
}