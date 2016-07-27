#!/bin/bash
mono tools/NuGet/nuget.exe install FAKE -OutputDirectory tools -ExcludeVersion
mono tools/NuGet/nuget.exe install NUnit.ConsoleRunner -OutputDirectory tools -ExcludeVersion
mono tools/FAKE/tools/FAKE.exe build.fsx
