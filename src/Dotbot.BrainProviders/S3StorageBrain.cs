
using Amazon;
using Amazon.Runtime;
using Dotbot.Diagnostics;
using Foundatio.Storage;

namespace Dotbot.BrainProviders
{
    public class S3StorageBrain : FileStorageBrain
    {
        public S3StorageBrain(AWSCredentials credentials, RegionEndpoint endpoint, string bucket, ILog log)
        : base(new S3FileStorage(credentials, endpoint, bucket), log)
        {
        }
    }
}