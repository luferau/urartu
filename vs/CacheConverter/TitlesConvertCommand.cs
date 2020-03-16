using System;
using System.IO;
using System.Linq;
using ManyConsole;

namespace CacheConverter
{
    // Map tiles format description
    // Source format
    // z{zoom}\{x_div_1024}\x{x}\{y_div_1024}\y{y}

    // Destination format
    // X {0} and Y {1} numbers and the Zoom level {2}
    // {0}_{1}_{2},
    public class TitlesConvertCommand : ConsoleCommand
    {
        private const int Success = 0;
        private const int Failure = 2;

        public string SourceFolder;
        public string DestinationFolder;

        public TitlesConvertCommand()
        {
            IsCommand("convert", "Convert map tile files name to Telerik LocalMapProvider files format.");
            HasRequiredOption("s|source=", "The full path to source folder.", s => SourceFolder = s);
            HasRequiredOption("d|destination=", "The full path to destination folder.", d => DestinationFolder = d);
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                var filePaths = Helper.GetFileList("*.*", SourceFolder).ToArray();
                Console.WriteLine($"{filePaths.Length} files found");

                var extension = "";
                var counter = 0;

                foreach (var filePath in filePaths)
                {
                    #region Progress report

                    counter++;
                    if (counter > 100)
                    {
                        Console.Write(".");
                        counter = 0;
                    }

                    #endregion

                    if (string.IsNullOrEmpty(extension))
                        extension = Path.GetExtension(filePath);

                    var pathParts = filePath.Split('\\');
                    var yPart = pathParts[pathParts.Length - 1];
                    var y = yPart.Substring(1, yPart.IndexOf('.') - 1);

                    var xPart = pathParts[pathParts.Length - 3];
                    var x = xPart.Substring(1, xPart.Length - 1);

                    var zPart = pathParts[pathParts.Length - 5];
                    var z = int.Parse(zPart.Substring(1, zPart.Length - 1)) - 1;

                    var newFileName = $"{x}_{y}_{z}{extension}";
                    var newFilePath = Path.Combine(DestinationFolder, newFileName);

                    if (File.Exists(newFilePath))
                        File.Delete(newFilePath);

                    File.Copy(filePath, newFilePath);
                }

                Console.WriteLine("\nConversion completed.");

                return Success;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);

                return Failure;
            }
        }
    }
}
