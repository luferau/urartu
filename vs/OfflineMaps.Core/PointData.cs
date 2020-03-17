namespace OfflineMaps.Core
{
    public class PointData
    {
        public bool Warning { get; set; }

        public double Latitude_deg { get; set; }
        public double Longitude_deg { get; set; }
        public double Altitude_m { get; set; }
        public double Speed_m_s { get; set; }

        public double PressureAir_Pa { get; set; }
        public double PressureOil_kgs_cm2 { get; set; }
        public double PressureFuel_kgs_cm2 { get; set; }
        public double Humidity_kg_m3 { get; set; }
        public double TemperatureAir_C { get; set; }
        public double RotationSpeedCrankshaft_turn_min { get; set; }

        public PointData(
            bool warning,
            double latitude_deg, 
            double longitude_deg, 
            double altitude_m, 
            double speed_m_s, 
            double pressureAir_Pa, 
            double pressureOil_kgs_cm2, 
            double pressureFuel_kgs_cm2, 
            double humidity_kg_m3, 
            double temperatureAir_C, 
            double rotationSpeedCrankshaft_turn_min)
        {
            Warning = warning;
            Latitude_deg = latitude_deg;
            Longitude_deg = longitude_deg;
            Altitude_m = altitude_m;
            Speed_m_s = speed_m_s;
            PressureAir_Pa = pressureAir_Pa;
            PressureOil_kgs_cm2 = pressureOil_kgs_cm2;
            PressureFuel_kgs_cm2 = pressureFuel_kgs_cm2;
            Humidity_kg_m3 = humidity_kg_m3;
            TemperatureAir_C = temperatureAir_C;
            RotationSpeedCrankshaft_turn_min = rotationSpeedCrankshaft_turn_min;
        }
    }
}
