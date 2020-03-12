using System.IO;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using System.Linq;

namespace MapNetControl
{
    public partial class MapNetControl: UserControl
    {
        public MapNetControl()
        {
            InitializeComponent();

            radMap.ShowSearchBar = false;

        }

        public void SetMapTitlesFolder(string titlesFolder)
        {
            if (radMap.Providers != null && radMap.Providers.Count > 0 && radMap.Providers[0] != null)
                radMap.Providers.Remove(radMap.Providers[0]);

            // Check titles extension
            var files = Directory.GetFiles(titlesFolder);
            if (files.Length == 0) return;
            var extension = ".jpg";
            var possibleFormats = new[] { ".jpg", ".png", ".bmp", ".jpeg" };

            foreach (var file in files)
            {
                extension = Path.GetExtension(file);

                // Check file extension is in possibleFormats
                if (possibleFormats.Contains(extension))
                {
                    // Stop searching, extension found
                    break;
                }
            }

            var provider = new LocalMapProvider
            {
                DirectoryPath = titlesFolder,
                // X {0} and Y {1} Zoom level {2}
                FileFormat = @"z{2}_x{0}_y{1}" + extension,
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
