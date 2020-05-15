using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CompressImagesFunction
{
    public partial class CompressionResult
    {
        public static CompressionResult[] Merge(CompressionResult[] newOptimizedImages, CompressionResult[] previousCommitResults)
        {
            List<CompressionResult> list = new List<CompressionResult>();
            list.AddRange(newOptimizedImages);
            list.AddRange(previousCommitResults);

            var nonRepeat = list.GroupBy(x => x.Title).Select(y => y.First());

            return nonRepeat.ToArray();
        }

        public static CompressionResult[] Filter(CompressionResult[] optimizedImages, string[] toRemove)
        {
            var relativePaths = toRemove.Select(path => Path.DirectorySeparatorChar + Path.GetFileName(path));
            var filtered = optimizedImages.Where(r => !relativePaths.Contains(r.Title));
            return filtered.ToArray();
        }
    }
}
