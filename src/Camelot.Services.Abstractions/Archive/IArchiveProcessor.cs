using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camelot.Services.Abstractions.Archive
{
    public interface IArchiveProcessor
    {
        // TODO: support progress
        Task PackAsync(IReadOnlyList<string> files, IReadOnlyList<string> directories, string sourceDirectory,
            string outputFile);

        Task ExtractAsync(string archivePath, string outputDirectory);
    }
}