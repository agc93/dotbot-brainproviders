#load "./scripts/helpers.cake"
#load "./scripts/version.cake"

var target = Argument<string>("target", "Default");
var configuration = Argument<string>("configuration", "Release");
var version = new BuildVersion("0.1.0", "local");

var solutionPath = File("./src/Dotbot.BrainProviders.sln");
var solution = ParseSolution(solutionPath);
var projects = GetProjects(solutionPath);
var artifacts = "./artifacts/";
var testResultsPath = MakeAbsolute(Directory(artifacts + "./test-results"));

var isRunningOnAppVeyor = BuildSystem.AppVeyor.IsRunningOnAppVeyor;
var isPullRequest = BuildSystem.AppVeyor.Environment.PullRequest.IsPullRequest;

Setup(context => 
{
    // Calculate semantic version.
    version = GetVersion(context);

    // Output some information.
    Information("Version: {0}", version.GetSemanticVersion());
    Information("Pull Request: {0}", isPullRequest);
});

Task("Clean")
    .Does(() =>
{
    CleanDirectory("./.artifacts");
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore(solutionPath, new DotNetCoreRestoreSettings
    {
        Verbose = false,
        Sources = new [] { "https://api.nuget.org/v3/index.json" }
    });
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() =>
{
    // Build
    DotNetCoreBuild(solutionPath, new DotNetCoreBuildSettings 
    {
        Configuration = configuration,
        ArgumentCustomization = args => args.Append("/p:Version={0}", version.GetSemanticVersion())
    });
});

Task("Pack")
    .IsDependentOn("Build")
    .Does(() =>
{
    // Publish
    foreach(var project in projects.SourceProjects)
    {
        Information("\nPacking {0}...", project.Path);
		var versionNotes = ParseAllReleaseNotes("./ReleaseNotes.md").FirstOrDefault(v => v.Version.ToString() == versionInfo.MajorMinorPatch);
		var releaseNotes = versionNotes != null ? string.Join(Environment.NewLine, versionNotes) : string.Empty;
        DotNetCorePack(project.Path.FullPath, new DotNetCorePackSettings 
        {
            Configuration = configuration,
            OutputDirectory = artifacts,
            VersionSuffix = version.Suffix,
            NoBuild = true,
            Verbose = false,
            ArgumentCustomization = args => args
                .Append("/p:Version={0}", version.GetSemanticVersion())
				.Append("/p:PackageReleaseNotes=\"{0}\"", releaseNotes)
                .Append("--include-symbols --include-source")
        });
    }
});

Task("Default")
    .IsDependentOn("Pack");

RunTarget(target);