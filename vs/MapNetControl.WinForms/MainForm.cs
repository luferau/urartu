using System.Windows.Forms;

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
            mapNetControl1.AddPin(52.5076682, 13.286064, "Berlin");
        }
    }
}
