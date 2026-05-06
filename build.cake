var target = Argument("target", "Default");

var projectDir = "./Project";
var latexDir = "./latex";
var doxyfile = "./Doxyfile";

Task("Clean")
    .Does(() =>
{
    CleanDirectory("./html");
    CleanDirectory("./latex");
});

Task("Doxygen")
    .Does(() =>
{
    StartProcess("doxygen", new ProcessSettings {
        Arguments = doxyfile
    });
});

Task("Latex")
    .IsDependentOn("Doxygen")
    .Does(() =>
{
    StartProcess("cmd", new ProcessSettings {
        Arguments = $"/C cd {latexDir} && make.bat"
    });
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Doxygen")
    .IsDependentOn("Latex");

Task("Default")
    .IsDependentOn("Build");

RunTarget(target);
