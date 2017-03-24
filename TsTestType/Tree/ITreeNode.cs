namespace TsTestType.Tree
{
    public interface ITreeNode
    {
        string FullPath { get; }
        bool Expanded { get; set; }
        bool Selected { get; set; }
        int ImageIndex { get; set; }
    }
}