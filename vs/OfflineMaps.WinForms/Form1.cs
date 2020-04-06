using System;
using System.Windows.Forms;
using OfflineMaps.Core;

namespace OfflineMaps.WinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            offlineMapControl.SetMapTitlesFolder(@"e:\UPWORK\urartu project\maps\sat\");
            //offlineMapControl.SetMapTitlesFolder(@"d:\upwork\urartu project\maps\google_map\");
        }

        private void point1Button_Click(object sender, EventArgs e)
        {
            var point1 = new PointData(false, 53.898727, 27.559064, 320, 65.5, 0, 0, 0, 0, 0, 0);

            offlineMapControl.AddPoint(point1, "Minsk");
        }

        private void point2button_Click(object sender, EventArgs e)
        {
            var point2 = new PointData(false, 52.898727, 29.559064, 320, 65.5, 0, 0, 0, 0, 0, 0);

            offlineMapControl.AddPoint(point2, "near Minsk");
        }
    }
}
