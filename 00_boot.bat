@echo off
cls
"tools\nuget\nuget.exe" "install" "FAKE" "-OutputDirectory" "tools" "-ExcludeVersion"
"tools\nuget\nuget.exe" "install" "NUnit.ConsoleRunner" "-OutputDirectory" "tools" "-ExcludeVersion"
"tools\nuget\nuget.exe" "install" "WiX.Toolset" "-OutputDirectory" "tools" "-ExcludeVersion"