using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace CacheConverter
{
    public class Program
    {
        public static IEnumerable<string> GetFileList(string fileSearchPattern, string rootFolderPath)
        {
            var pending = new Queue<string>();
            pending.Enqueue(rootFolderPath);

            while (pending.Count > 0)
            {
                rootFolderPath = pending.Dequeue();
                string[] tmp;
                try
                {
                    tmp = Directory.GetFiles(rootFolderPath, fileSearchPattern);
                }
                catch (UnauthorizedAccessException)
                {
                    continue;
                }

                foreach (var t in tmp)
                {
                    yield return t;
                }

                tmp = Directory.GetDirectories(rootFolderPath);
                foreach (var t in tmp)
                {
                    pending.Enqueue(t);
                }
            }
        }

        static void Main(string[] args)
        {
            // Source format
            // z{zoom}\{x_div_1024}\x{x}\{y_div_1024}\y{y}

            // Destination format
            // X {0} and Y {1} numbers and the Zoom level {2}
            // os_{0}_{1}_{2},

            /*
            var sourceFolder = @"g:\_soft\SASPlanet_160707\SAS.Planet.Release.160707\cache\map\";
            var destinationFolder = @"g:\_soft\SASPlanet_160707\SAS.Planet.Release.160707\cache\map_\";
            */

            var sourceFolder = @"d:\_soft\SAS.Planet.Release.190707\cache\map\";
            var destinationFolder = @"d:\upwork\urartu project\maps\google_map\";

            var filePaths = GetFileList("*.*", sourceFolder).ToArray();

            Console.WriteLine($"Source folder: {sourceFolder}");
            Console.WriteLine($"Destination folder: {destinationFolder}");
            Console.WriteLine($"{filePaths.Length} files found");

            var counter = 0;

            foreach (var filePath in filePaths)
            {
                counter++;
                if (counter > 100)
                {
                    Console.Write(".");
                    counter = 0;
                }

                //var ex = Path.GetExtension(filePath);

                var pathParts = filePath.Split('\\');
                var yPart = pathParts[pathParts.Length - 1];
                var y = yPart.Substring(1, yPart.IndexOf('.') - 1);
                
                var xPart = pathParts[pathParts.Length - 3];
                var x = xPart.Substring(1, xPart.Length - 1);

                var zPart = pathParts[pathParts.Length - 5];
                var z = int.Parse(zPart.Substring(1, zPart.Length - 1)) - 1;

                var newFileName = $"{x}_{y}_{z}.png";
                var newFilePath = $"{destinationFolder}{newFileName}";
                
                File.Copy(filePath, newFilePath);
            }

            Console.WriteLine("Conversion completed");
            Console.WriteLine("Press any key to exit..");
            Console.ReadKey();
        }
    }
}
