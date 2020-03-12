using System.Windows.Forms;

namespace MapNetControl.WinForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            mapNetControl1.SetMapTitlesFolder(@"e:\UPWORK\urartu\maps\sat\");
        }
    }
}
