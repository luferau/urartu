using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ManyConsole;
using Newtonsoft.Json;
using OfflineMaps.Core;

namespace OfflineMaps.Console
{
    public class TrackGenerateCommand : ConsoleCommand
    {
        private const int Success = 0;
        private const int Failure = 2;

        public string CoordinatesFilePath;
        public string TrackFileName;

        public TrackGenerateCommand()
        {
            IsCommand("trackgen", "Track generate with PointData's based on coordinates file");
            HasRequiredOption("c|coordinates=", "Full path to file with coordinates in format: longitude,latitude,altitude", c => CoordinatesFilePath = c);
            HasRequiredOption("t|track=", "Track file name", t => TrackFileName = t);
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                var coordinates = File.ReadAllLines(CoordinatesFilePath);

                var points = new List<PointData>();

                var rnd = new Random();

                foreach (var coordinate in coordinates)
                {
                    var parts = coordinate.Split(',');

                    var point = new PointData
                    {
                        Longitude_deg = double.Parse(parts[0], CultureInfo.InvariantCulture),
                        Latitude_deg = double.Parse(parts[1], CultureInfo.InvariantCulture),
                        Altitude_m = double.Parse(parts[2], CultureInfo.InvariantCulture),
                        Speed_m_s = 60 + rnd.Next(10),

                        PressureAir_Pa = 10 + rnd.Next(100)/10.0,
                        PressureOil_kgs_cm2 = 20 + rnd.Next(100) / 10.0,
                        PressureFuel_kgs_cm2 = 30 + rnd.Next(100) / 10.0,
                        Humidity_kg_m3 = 0.01 + rnd.Next(100) / 100.0,
                        TemperatureAir_C = 25 + rnd.Next(100) / 10.0,
                        RotationSpeedCrankshaft_turn_min = 4000 + rnd.Next(1000),
                    };

                    points.Add(point);
                }

                var folder = Path.GetDirectoryName(CoordinatesFilePath);
                var pointsFilePath = Path.Combine(folder, TrackFileName);

                var jsonString = JsonConvert.SerializeObject(points, Formatting.Indented);
                File.WriteAllText(pointsFilePath, jsonString);

                System.Console.WriteLine("\nTrack file generated.");

                return Success;
            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine(ex.Message);
                System.Console.Error.WriteLine(ex.StackTrace);

                return Failure;
            }
        }
    }
}
