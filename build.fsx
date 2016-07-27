#I @"tools/FAKE/tools/"
#r @"tools/FAKE/tools/FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile
open Fake.Git
open Fake.Testing.NUnit3
open System.IO

let projectName           = "Pokemon.Team.Builder"

//Directories
let buildDir                = @".\build\"
let domainBuildDir          = buildDir + @"domain\"
let appBuildDir             = buildDir + @"app\"

let deployDir               = @".\Publish\"
let domaindeployDir         = deployDir + @"domain\"
let appdeployDir            = deployDir + @"app\"

let testDir                 = @".\test"

let packagesDir             = @".\packages\"

let mutable version         = "1.0"
let mutable build           = buildVersion
let mutable nugetVersion    = ""
let mutable asmVersion      = ""
let mutable asmInfoVersion  = ""
let mutable setupVersion    = ""

let gitbranch = Git.Information.getBranchName "."
let sha = Git.Information.getCurrentHash()

Target "Clean" (fun _ ->
    CleanDirs [buildDir; deployDir; testDir]
)

Target "RestorePackages" (fun _ ->
   RestorePackages()
)

Target "BuildVersions" (fun _ ->

    let safeBuildNumber = if not isLocalBuild then build else "0"

    asmVersion      <- version + "." + safeBuildNumber
    asmInfoVersion  <- asmVersion + " - " + gitbranch + " - " + sha

    nugetVersion    <- version + "." + safeBuildNumber
    setupVersion    <- version + "." + safeBuildNumber

    match gitbranch with
        | "master" -> ()
        | "develop" -> (nugetVersion <- nugetVersion + " - " + "beta")
        | _ -> (nugetVersion <- nugetVersion + " - " + gitbranch)

    SetBuildNumber nugetVersion
)
Target "AssemblyInfo" (fun _ ->
    BulkReplaceAssemblyInfoVersions "src/" (fun f ->
                                              {f with
                                                  AssemblyVersion = asmVersion
                                                  AssemblyInformationalVersion = asmInfoVersion})
)

Target "BuildApp" (fun _->
    !! @"src\app\**\*.csproj"
      |> MSBuildRelease appBuildDir "Build"
      |> Log "Build - Output: "
)

Target "BuildDomain" (fun _->
    !! @"src\domain\**\*.csproj"
      |> MSBuildRelease domainBuildDir "Build"
      |> Log "Build - Output: "
)

Target "BuildTest" (fun _ ->
    !! @"src\test\**\*.csproj"
      |> MSBuildDebug testDir "Build"
      |> Log "Build Log: "
)

Target "NUnit" (fun _ ->
    let testFiles = !!(testDir @@ @"\**\*.Tests.dll")
    
    if testFiles.Includes.Length <> 0 then
      testFiles
        |> NUnit3 (fun test ->
             {test with
                   Framework = Net45;
                   ShadowCopy = false;})
)

Target "Publish" (fun _ ->
    CreateDir appdeployDir
    CreateDir domaindeployDir

    !! (appBuildDir @@ @"/**/*.* ")
      -- " *.pdb"
        |> CopyTo appdeployDir 

    !! (domainBuildDir @@ @"/**/*.* ")
      -- " *.pdb"
        |> CopyTo domaindeployDir
)

Target "Zip" (fun _ ->
    !! (buildDir @@ @"\**\*.* ")
        -- " *.zip"
            |> Zip appBuildDir (deployDir + projectName + "." + version + ".zip")
)

"Clean"
  ==> "RestorePackages"
  ==> "BuildVersions"
  =?> ("AssemblyInfo", not isLocalBuild )
  ==> "BuildDomain"
  ==> "BuildApp"
  ==> "BuildTest"
  ==> "NUnit"
  ==> "Zip"
  ==> "Publish"

RunTargetOrDefault "Publish"
