using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using System.Linq;
using Telerik.WinControls.UI.Map;

namespace MapNetControl
{
    public partial class MapNetControl: UserControl
    {
        public MapNetControl()
        {
            InitializeComponent();

            radMap.ShowSearchBar = false;

            // Create map layers
            SetupLayers();

        }
        private void SetupLayers()
        {
            var pathLayer = new MapLayer("Path");
            radMap.Layers.Add(pathLayer);

            MapLayer pinsLayer = new MapLayer("Pins");
            radMap.Layers.Add(pinsLayer);
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

        public void AddPin(double latitude_deg, double longitude_deg, string text)
        {
            var pin = new MapPin(new PointG(latitude_deg, longitude_deg));
            pin.ToolTipText = text;
            pin.BackColor = Color.FromArgb(11, 195, 197);
            radMap.Layers["Pins"].Add(pin);
        }

    }
}
