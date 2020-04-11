using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CompressImagesFunction
{
    public class CompressionResult
    {
        public string Title { get; set; }

        public string OriginalPath { get; set; }

        public double SizeBefore { get; set; }

        public double SizeAfter { get; set; }

        public double PercentSaved => (1 - (SizeAfter / SizeBefore)) * 100d;

        public override string ToString() =>
            $"{Title.Replace('\\', '/')} -- {SizeBefore:N2}kb -> {SizeAfter:N2}kb ({PercentSaved:0.##}%)";

        public static CompressionResult[] ParseCommitMessage(string commitBody)
        {
            try
            {
                Console.WriteLine("COMMIT BODY: {0}", commitBody);

                var compressionResults = new List<CompressionResult>();

                var commitLines = commitBody.Split(new[] { '\r', '\n' });
                for (var i = 0; i < commitLines.Length; i++)
                {
                    if (i == 0 || string.IsNullOrWhiteSpace(commitLines[i]))
                    {
                        // skip the first line and blank lines
                        continue;
                    }

                    if (commitLines[i].StartsWith("Signed-off-by:") || commitLines[i].StartsWith("*Total --"))
                    {
                        // skip the DCO line
                        continue;
                    }

                    var pattern = @"\*?(.*) -- (.*)kb -> (.*)kb \((.*)%\)";
                    var capture = Regex.Matches(commitLines[i], pattern)[0];

                    compressionResults.Add(new CompressionResult
                    {
                        Title = capture.Groups[1].Value,
                        SizeBefore = Convert.ToDouble(capture.Groups[2].Value),
                        SizeAfter = Convert.ToDouble(capture.Groups[3].Value),
                    });
                }

                return compressionResults.ToArray();
            }
            catch
            {
                Console.WriteLine("PARSING FAILED!");
                // commit messages can be out of our control
                return null;
            }
        }

        public static CompressionResult[] Merge(CompressionResult[] newOptimizedImages, CompressionResult[] previousCommitResults)
        {
            List<CompressionResult> list = new List<CompressionResult>();
            list.AddRange(newOptimizedImages);
            list.AddRange(previousCommitResults);

            var nonRepeat = list.GroupBy(x => x.Title).Select(y => y.First());

            return nonRepeat.ToArray();
        }

        public static CompressionResult[] Filter(CompressionResult[] OptimizedImages, string[] toRemove)
        {
            List<CompressionResult> list = new List<CompressionResult>(OptimizedImages);

            var filtered = list.Where(r => !toRemove.Contains(r.Title));

            return filtered.ToArray();
        }
    }
}
