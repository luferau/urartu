using System.Windows.Forms;
using MapNetControl.Urartu;

namespace MapNetControl.WinForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            mapNetControl1.SetMapTitlesFolder(@"e:\UPWORK\urartu project\maps\sat\");
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            var point1 = new PointData(false, 53.898727, 27.559064, 320, 65.5, 0,0,0,0,0,0);

            mapNetControl1.AddPoint(point1, "Minsk");
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            var point2 = new PointData(false, 52.898727, 29.559064, 320, 65.5, 0, 0, 0, 0, 0, 0);

            mapNetControl1.AddPoint(point2, "near Minsk");
        }
    }
}
