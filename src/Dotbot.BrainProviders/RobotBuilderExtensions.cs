using Dotbot.Diagnostics;
using Foundatio.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Dotbot.BrainProviders
{
    public static class RobotBuilderExtensions
    {
        
        public static RobotBuilder UseFileSystemBrain(this RobotBuilder builder, string folderPath = null) {
            folderPath = folderPath ?? System.IO.Directory.GetCurrentDirectory();
            builder.Services.AddSingleton<IBrainProvider>(provider => new FileSystemBrain(folderPath, provider.GetService<ILog>()));
            return builder;
        }

        public static RobotBuilder UseS3StorageBrain(this RobotBuilder builder,
            Amazon.Runtime.AWSCredentials credentials,
            Amazon.RegionEndpoint endpoint,
            string bucket = "dotbot") {
            builder.Services.AddSingleton<IBrainProvider>(provider => {
                return new S3StorageBrain(
                    credentials,
                    endpoint,
                    bucket,
                    provider.GetService<ILog>()
                );
            });
            return builder;
        }
        
    }
}