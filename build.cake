var target = Argument("target", "Default");

var projectDir = "./Project";
var latexDir = "./latex";
var doxyfile = "./Doxyfile";
var solutionFile = "./Project/Midterm.sln";



Task("StyleCop")
	.Does(() =>
	{
		var result = StartProcess("dotnet", new ProcessSettings {
        	Arguments = $"format \"{solutionFile}\" --verify-no-changes --verbosity diagnostic"
    	});

    	if (result != 0)
        	throw new Exception("Style check failed. Please run dotnet format manually.");
});

Task("Clean")
    .Does(() =>
{
    CleanDirectory("./html");
    CleanDirectory("./latex");
	DotNetClean(solutionFile);
});

// Builds

Task("BuildDebug")
    .IsDependentOn("StyleCop")
    .IsDependentOn("Clean")
	.Does(() => {
		DotNetBuild(solutionFile, new DotNetBuildSettings {
			Configuration = "Debug",
			NoRestore = false
    	});
	}
);

Task("BuildRelease")
    .IsDependentOn("StyleCop")
    .IsDependentOn("Clean")
	.Does(() => {
		DotNetBuild(solutionFile, new DotNetBuildSettings {
			Configuration = "Release",
			NoRestore = false
    	});
	}
);

//Documentation

Task("Doxygen")
    .Does(() =>
{
	var result = StartProcess("doxygen", new ProcessSettings {
        Arguments = doxyfile
    });

    if (result != 0)
        throw new Exception("Doxygen failed.");
});

Task("Latex")
    .IsDependentOn("Doxygen")
    .Does(() =>
{
    var result = StartProcess("cmd", new ProcessSettings {
        Arguments = $"/C cd {latexDir} && make.bat"
    });

    if (result != 0)
        throw new Exception("LaTeX failed.");

});

// Testing
Task("Test")
	.IsDependentOn("BuildDebug")
    .Does(() =>
{
	DotNetTest(solutionFile, new DotNetTestSettings {
        Configuration = "Debug",
        NoBuild = true,
        Collectors = new[] { "XPlat Code Coverage" },
        Loggers = new[] { "trx" }
    });
});

Task("Docs")
    .IsDependentOn("Latex");

Task("Build")
    .IsDependentOn("BuildDebug")
    .IsDependentOn("BuildRelease")
    .IsDependentOn("Docs");

Task("Default")
    .IsDependentOn("Build")
    .IsDependentOn("Test");

RunTarget(target);
