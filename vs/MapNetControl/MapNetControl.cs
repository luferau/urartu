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
                FileFormat = @"{0}_{1}_{2}" + extension,
                MinZoomLevel = 1,
                MaxZoomLevel = 20
            };
            radMap.Providers?.Add(provider);
        }

        #region Pins

        private void AddPinLocal(double latitude_deg, double longitude_deg, string text, int r, int g, int b)
        {
            var pin = new MapPin(new PointG(latitude_deg, longitude_deg))
            {
                ToolTipText = text,
                BackColor = Color.FromArgb(r, g, b)
            };
            radMap.Layers["Pins"].Add(pin);
        }

        public void AddPin(double latitude_deg, double longitude_deg, string text)
        {
            AddPinLocal(latitude_deg, longitude_deg, text, 11, 195, 197);
        }

        public void AddPin(double latitude_deg, double longitude_deg, string text, int r, int g, int b)
        {
            AddPinLocal(latitude_deg, longitude_deg, text, r, g, b);
        }

        public void ClearPins()
        {
            radMap.Layers["Pins"].Clear();
        }

        #endregion

        #region Path

        private MapVisualElement CreatePathSegment(PointG start, PointG end, Color color, int width, int dashStyle)
        {
            var landRoute = new MapRoute(start, end)
            {
                BorderColor = color,
                BorderWidth = width,
                BorderDashStyle = (System.Drawing.Drawing2D.DashStyle)dashStyle,
                SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias
            };

            return landRoute;
        }

        private bool _pathStarted;
        private PointG _previousPoint;

        public void AddPathPoint(double latitude_deg, double longitude_deg)
        {
            if (!_pathStarted)
            {
                _previousPoint = new PointG(latitude_deg, longitude_deg);
                _pathStarted = true;
                return;
            }

            var nextPoint = new PointG(latitude_deg, longitude_deg);
            var pathElement = CreatePathSegment(_previousPoint, nextPoint, Color.Red, 3, 0);

            radMap.Layers["Path"].Add(pathElement);

            _previousPoint = nextPoint;
        }

        public void ClearPath()
        {
            radMap.Layers["Path"].Clear();
        }

        #endregion
    }
}
