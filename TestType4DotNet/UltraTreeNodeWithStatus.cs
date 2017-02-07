using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infragistics.Win.UltraWinTree;

namespace CTSAddin
{
    public enum StatusNode
    {
        Test,
        NotFilled,//status folder node
        Filled    //status folder node
    }

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
