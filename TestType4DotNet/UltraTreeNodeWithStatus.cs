using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infragistics.Win.UltraWinTree;

namespace CTSAddin
{
    /// <summary>
    /// Status node for filling data step by step on request
    /// </summary>
    public enum StatusNode
    {
        Test,
        NotFilled,//status folder node
        Filled    //status folder node
    }
    /// <summary>
    /// Node for dictionary for filling data step by step on request
    /// </summary>
    public class UltraTreeNodeWithStatus
    {
        public UltraTreeNode Node { get; set; }
        public StatusNode Status { get; set; }

        public UltraTreeNodeWithStatus(UltraTreeNode node, StatusNode status)
        {
            Node = node;
            Status = status;
        }
    }
}
