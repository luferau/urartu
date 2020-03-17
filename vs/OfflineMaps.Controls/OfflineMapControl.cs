using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Windows.Forms;
using OfflineMaps.Core;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Map;

namespace OfflineMaps.Controls
{
    public partial class OfflineMapControl: UserControl
    {
        private readonly ResourceManager _resourceManager;

        public OfflineMapControl()
        {
            InitializeComponent();

            radMap.ShowSearchBar = false;

            // Create map layers
            SetupLayers();

            radMap.MapElement.ViewportChanged += MapElementOnViewportChanged;
            radMap.Click += RadMapOnClick;

            _resourceManager = new ResourceManager(typeof(OfflineMapControl));
        }

        #region Events

        private void MapElementOnViewportChanged(object sender, ViewportChangedEventArgs e)
        {
            if ((e.Action & ViewportChangeAction.Zoom) != 0)
            {
                radMap.Layers["Pins"].IsVisible = radMap.MapElement.ZoomLevel <= 15;
                radMap.Layers["Callout"].IsVisible = radMap.MapElement.ZoomLevel <= 15;
                radMap.Layers["Labels"].IsVisible = radMap.MapElement.ZoomLevel > 15;
            }
        }

        private void RadMapOnClick(object sender, EventArgs e)
        {
            var args = e as MouseEventArgs;
            radMap.Layers["Callout"].Clear();

            var point = new PointL(args.X - radMap.MapElement.PanOffset.Width, args.Y - radMap.MapElement.PanOffset.Height);
            var pin = radMap.Layers.HitTest(point) as MapPin;

            if (!(pin?.Tag is PointData pointData)) return;

            var calloutText = $"Скорость, м/с: {pointData.Speed_m_s}\n" +
                              $"Высота, м: {pointData.Altitude_m}\n" +
                              $"\nДавление:\n" +
                              $"Наддувочное давление воздуха, Па: {pointData.PressureAir_Pa}\n" +
                              $"Давление масла, кгс/см^2: {pointData.PressureOil_kgs_cm2}\n" +
                              $"Давление топлива, кгс/см^2: {pointData.PressureFuel_kgs_cm2}\n" +
                              $"\nВлажность, кг/м^3: {pointData.Humidity_kg_m3}\n" +
                              $"\nТемпература, C: {pointData.TemperatureAir_C}\n" +
                              $"\nЧастота вр. кол. вала, об/мин: {pointData.RotationSpeedCrankshaft_turn_min}";

            var calloutColor = pointData.Warning ? Helper.WarningColor : Helper.NormalColor;
            var calloutImage = (Image)(pointData.Warning ? _resourceManager.GetObject("truck_warning")
                                                         : _resourceManager.GetObject("truck_ok"));

            var callout = new MapCallout(pin)
            {
                ForeColor = Color.White,
                BackColor = calloutColor,
                BorderColor = Color.White,
                Image = calloutImage,
                Text = calloutText
            };
            radMap.Layers["Callout"].Add(callout);
        }

        #endregion

        private void SetupLayers()
        {
            var pinsLayer = new MapLayer("Pins");
            radMap.Layers.Add(pinsLayer);

            var pathLayer = new MapLayer("Path");
            radMap.Layers.Add(pathLayer);

            var labelsLayer = new MapLayer("Labels")
            {
                IsVisible = false
            };
            radMap.Layers.Add(labelsLayer);

            var calloutLayer = new MapLayer("Callout");
            radMap.Layers.Add(calloutLayer);
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

        #region Points

        private static int schetchik;

        public void AddPoint(PointData point, string text)
        {
            schetchik++;
            if (schetchik > 60) return;

            var color = point.Warning ? Helper.WarningColor : Helper.NormalColor;
            var image = (Image) (point.Warning ? _resourceManager.GetObject("truck_warning") 
                                               : _resourceManager.GetObject("truck_ok"));
            
            // Add pin
            AddPinLocal(
                point.Latitude_deg,
                point.Longitude_deg,
                point,
                text,
                color);

            // Add label
            var labelText = $"Скорость, м/с: {point.Speed_m_s}\n" +
                            $"Высота, м: {point.Altitude_m}\n" +
                            $"Частота вр. кол. вала, об/мин: {point.RotationSpeedCrankshaft_turn_min}";

            AddLabelLocal(
                point.Latitude_deg,
                point.Longitude_deg,
                labelText,
                color,
                image);

            // Add path
            AddPathPoint(
                point.Latitude_deg,
                point.Longitude_deg,
                color);
        }

        public void ClearPoints()
        {
            schetchik = 0;

            ClearPins();
            ClearLabels();
            ClearPath();
        }

        #endregion

        #region Pins

        private void AddPinLocal(double latitude_deg, double longitude_deg, object tag, string text, Color color)
        {
            var pin = new MapPin(new PointG(latitude_deg, longitude_deg))
            {
                Tag = tag,
                ToolTipText = text,
                BackColor = color
            };
            radMap.Layers["Pins"].Add(pin);
        }

        /*
        public void AddPin(double latitude_deg, double longitude_deg, string text)
        {
            AddPinLocal(latitude_deg, longitude_deg, null, text, Color.FromArgb(11, 195, 197));
        }

        public void AddPin(double latitude_deg, double longitude_deg, string text, int r, int g, int b)
        {
            AddPinLocal(latitude_deg, longitude_deg, null, text, Color.FromArgb(r, g, b));
        }
        */

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

        private void AddPathPoint(double latitude_deg, double longitude_deg, Color color)
        {
            if (!_pathStarted)
            {
                _previousPoint = new PointG(latitude_deg, longitude_deg);
                _pathStarted = true;
                return;
            }

            var nextPoint = new PointG(latitude_deg, longitude_deg);
            var pathElement = CreatePathSegment(_previousPoint, nextPoint, color, 3, 0);

            radMap.Layers["Path"].Add(pathElement);

            _previousPoint = nextPoint;
        }

        public void ClearPath()
        {
            _pathStarted = false;

            radMap.Layers["Path"].Clear();
        }

        #endregion

        #region Labels

        private void AddLabelLocal(double latitude_deg, double longitude_deg, string text, Color color, Image image)
        {
            var label = new MapLabel(new PointG(latitude_deg, longitude_deg), text)
            {
                // Add transparence 
                BackColor = Color.FromArgb(100, color.R, color.G, color.B),
                BorderColor = Color.White,
                ForeColor = Color.White,
                Image = image
            };
            radMap.Layers["Labels"].Add(label);
        }

        public void ClearLabels()
        {
            radMap.Layers["Labels"].Clear();
        }

        #endregion
    }
}
