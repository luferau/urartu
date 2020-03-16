using System.Windows.Forms;
using Telerik.WinControls.UI.Map;

namespace MapNetControl.WinForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            mapNetControl1.SetMapTitlesFolder(@"d:\upwork\urartu project\maps\google_map\");
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            mapNetControl1.AddPin(53.898727, 27.559064, "Minsk");
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            
        }
    }
}
