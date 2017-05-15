# Dotbot.BrainProviders

A package of alternative brain providers for the [Dotbot framework](https://github.com/botnetcore/dotbot) for persisting data. This package adds support for backing your bot with storage from the local file system, Amazon S3, or Azure Blob Storage (coming soon).

Powered by [Foundatio](https://github.com/exceptionless/Foundatio).

## Getting Started

### Local file system

To add the local file system provider to your bot, just update your startup code: 

```csharp
// Build the robot.
var robot = new RobotBuilder()
    // ...
    .UseFileSystemBrain()
    // ...
    .Build();
```

You can optionally provide a folder path, but the provider will default to the current working directory:

```csharp
// Build the robot.
var robot = new RobotBuilder()
    // ...
    .UseFileSystemBrain("/tmp/bot")
    // ...
    .Build();
```

### Amazon S3

To add the S3 storage provider, you will need to provide a bit more information.

First, you will need to have configured your AWS Credentials as outlined in the [official documentation](http://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-creds.html) and then pass an `AWSCredentials` object, and your chosen `RegionEndpoint`. You can also provide the name of a bucket to use (optional, defaults to `"dotbot"`):

```csharp
AWSCredentials creds;
// Build the robot.
var robot = new RobotBuilder()
    // ...
    .UseS3StorageBrain(creds, RegionEndpoint.USWest1, "botstorage")
    // ...
    .Build();
```