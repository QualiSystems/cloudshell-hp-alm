using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TsTestType
{
    public partial class RegisterErrorForm : Form
    {
        public RegisterErrorForm(string title, string explain, string error)
        {
            InitializeComponent();
            Text = title;
            lblExplain.Text = explain;
            lblError.Text = error;
            lblError.Visible = !string.IsNullOrEmpty(error);

        }
    }
}
