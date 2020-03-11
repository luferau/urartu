using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace MapNetControl
{
    public partial class MapNetControl: UserControl
    {
        public MapNetControl()
        {
            InitializeComponent();

            radMap.ShowSearchBar = false;

        }

        public void SetMapTitlesFolder(string path)
        {
            if (radMap.Providers != null && radMap.Providers.Count > 0 && radMap.Providers[0] != null)
                radMap.Providers.Remove(radMap.Providers[0]);

            var provider = new LocalMapProvider
            {
                DirectoryPath = path,
                // X {0} and Y {1} Zoom level {2}
                FileFormat = @"z{2}_x{0}_y{1}.png",
                MinZoomLevel = 1,
                MaxZoomLevel = 20
            };
            radMap.Providers?.Add(provider);
        }

        public void ShowPath()
        {

        }

        private void SetupLayers()
        {

        }
    }
}
