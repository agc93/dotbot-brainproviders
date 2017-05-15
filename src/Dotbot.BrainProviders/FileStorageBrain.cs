using System;
using System.IO;
using Dotbot;
using Dotbot.Diagnostics;
using Foundatio.Storage;

namespace Dotbot.BrainProviders
{

    public abstract class FileStorageBrain : IBrainProvider
    {
        protected FileStorageBrain(IFileStorage storage, ILog log)
        {
            Log = log;
        }
        protected IFileStorage Storage { get; private set; }
        protected ILog Log { get; private set; }

        public virtual void Connect()
        {
            Log.Information("File Storage Brain connected.");
        }
        public virtual void Disconnect()
        {
            Log.Information("File Storage Brain disconnected.");
        }
        public virtual string Get(string key)
        {
            var contents = Storage.GetFileContentsAsync($"{key}.data").GetAwaiter().GetResult();
            return contents;
        }
        public virtual void Set(string key, string data)
        {
            using (var s = GenerateStreamFromString(data)) {
                var saved = Storage.SaveFileAsync("${key}.data", s).GetAwaiter().GetResult();
            }
        }

        protected static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
