using Infragistics.Win.UltraWinTree;

namespace TsTestType
{
    /// <summary>
    /// Status node for filling data step by step on request
    /// </summary>
    public enum StatusNode
    {
        // The node is a test
        Test,
        
        // The folder node was not filled yet
        NotFilled,

        // The folder node was filled
        Filled
    }

    /// <summary>
    /// Node for dictionary for filling data step by step on request
    /// </summary>
    public class UltraTreeNodeWithStatus
    {
        public UltraTreeNode Node { get; private set; }
        public StatusNode Status { get; set; }

        public UltraTreeNodeWithStatus(UltraTreeNode node, StatusNode status)
        {
            Node = node;
            Status = status;
        }
    }
}
