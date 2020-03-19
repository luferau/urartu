using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using ManyConsole;

namespace OfflineMaps.Console
{
    public class TilesMergeCommand : ConsoleCommand
    {
        private const int Success = 0;
        private const int Failure = 2;

        public string Source1 { get; set; }
        public string Source2 { get; set; }
        public string DestinationFolder { get; set; }

        public TilesMergeCommand()
        {
            IsCommand("merge", "Merge map tile files.");
            HasRequiredOption("s1|source1=", "The full path to source 1 folder.", s1 => Source1 = s1);
            HasRequiredOption("s2|source2=", "The full path to source 2 folder.", s2 => Source2 = s2);
            HasRequiredOption("d|destination=", "The full path to destination folder.", d => DestinationFolder = d);
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                var image1 = Image.FromFile(Source1);
                var image2 = Image.FromFile(Source2);

                var target = new Bitmap(image1.Width, image2.Height, PixelFormat.Format32bppArgb);
                var graphics = Graphics.FromImage(target);

                graphics.CompositingMode = CompositingMode.SourceOver; // this is the default, but just to be clear

                graphics.DrawImage(image1, 0, 0);
                graphics.DrawImage(image2, 0, 0);

                target.Save("filename.png", ImageFormat.Png);

                System.Console.WriteLine("\nMerging completed.");

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
