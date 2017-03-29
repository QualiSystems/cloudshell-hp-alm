namespace TsTestType.Tree
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
    public class TreeNodeWithStatus : ITreeNodeWithStatus
    {
        public ITreeNode Node { get; private set; }
        public StatusNode Status { get; set; }

        public TreeNodeWithStatus(ITreeNode node, StatusNode status)
        {
            Node = node;
            Status = status;
        }
    }

    public interface ITreeNodeWithStatus
    {
        ITreeNode Node { get; }
        StatusNode Status { get; set; }
    }
}
