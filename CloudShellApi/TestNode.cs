using System.Collections.Generic;

namespace QS.ALM.CloudShellApi
{
    public enum TypeNode
    {
        Folder,
        Test
    }
    
    public class TestNode
    {
        public string Name { get; private set; }
        public List<TestNode> Children { get; private set; }

        public TypeNode Type { get; set; } // Test or Folder

        public TestNode(string name, TypeNode type)
        {
            Name = name;
            Type = type;
            Children = new List<TestNode>();
        }

       
    }            
}