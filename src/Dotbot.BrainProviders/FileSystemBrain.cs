using Dotbot.Diagnostics;
using Foundatio.Storage;

namespace Dotbot.BrainProviders
{
    public class FileSystemBrain : FileStorageBrain
    {
        public FileSystemBrain(string folderPath, ILog log) : base(new FolderFileStorage(folderPath), log)
        {

        }
    }
}