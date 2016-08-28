#I @"tools/FAKE/tools/"
#r @"tools/FAKE/tools/FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile
open Fake.Git
open Fake.Testing.NUnit3
open System.IO
open System

let projectName           = "Pokemon.Team.Builder"

//Directories
let buildDir                = @"build/"
let domainBuildDir          = buildDir + @"domain/"
let appBuildDir             = buildDir + @"app/"

let deployDir               = @"Publish/"
let domaindeployDir         = deployDir + @"domain/"
let appdeployDir            = deployDir + @"app/"

let testDir                 = @"test"

let packagesDir             = @"packages/"

let mutable version         = "1.0"
let mutable build           = buildVersion
let mutable nugetVersion    = ""
let mutable asmVersion      = ""
let mutable asmInfoVersion  = ""
let mutable setupVersion    = ""

let gitbranch = Git.Information.getBranchName "."
let sha = Git.Information.getCurrentHash()

let WiXPath = Path.Combine("tools", "WiX.Toolset", "tools", "wix")
let WixProductUpgradeGuid = new Guid("10A24685-C830-42A5-B813-59651114DDE1")
let ProductVersion () = asmVersion
let setupFileName() = sprintf "%s - %s.msi" projectName (ProductVersion ())
let ProductPublisher = "Kroenert"

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

Target "BuildSetup" (fun _ ->
    // This defines, which files should be collected when running bulkComponentCreation
    let fileFilter = fun (file : FileInfo) -> true
        
    // Collect Files which should be shipped. Pass directory with your deployment output for deployDir
    // along with the targeted architecture.
    let components = bulkComponentCreation fileFilter (DirectoryInfo appdeployDir) Architecture.X64
             
    // Collect component references for usage in features
    let componentRefs = components |> Seq.map(fun comp -> comp.ToComponentRef())

    let completeFeature = generateFeatureElement (fun f -> 
                                                    {f with  
                                                        Id = "Complete"
                                                        Title = "Complete Feature"
                                                        Level = 1 
                                                        Description = "Installs all features"
                                                        Components = componentRefs
                                                        Display = Expand 
                                                    })

    // Generates a predefined WiX template with placeholders which will be replaced in "FillInWiXScript"
    generateWiXScript "SetupTemplate.wxs"

    let WiXUIMondo = generateUIRef (fun f ->
                                        {f with
                                            Id = "WixUI_Mondo"
                                        })

    let WiXUIError = generateUIRef (fun f ->
                                        {f with
                                            Id = "WixUI_ErrorProgressText"
                                        })

    let MajorUpgrade = generateMajorUpgradeVersion(
                            fun f ->
                                {f with 
                                    Schedule = MajorUpgradeSchedule.AfterInstallExecute
                                    DowngradeErrorMessage = "A later version is already installed, exiting."
                                })

    FillInWiXTemplate "" (fun f ->
                            {f with
                                // Guid which should be generated on every build
                                ProductCode = Guid.NewGuid()
                                ProductName = "Pokemon Team Builder"
                                Description = "Application for building Pokemon Teams"
                                ProductLanguage = 1031
                                ProductVersion = ProductVersion()
                                ProductPublisher = ProductPublisher
                                // Set fixed upgrade guid, this should never change for this project!
                                UpgradeGuid = WixProductUpgradeGuid
                                MajorUpgrade = [MajorUpgrade]
                                UIRefs = [WiXUIMondo; WiXUIError]
                                ProgramFilesFolder = ProgramFiles64
                                Components = components
                                BuildNumber = build
                                Features = [completeFeature]
                            })
        

    // run the WiX tools
    WiX (fun p -> {p with ToolDirectory = WiXPath}) 
        (Path.Combine (deployDir, (setupFileName ())))
        @".\SetupTemplate.wxs"
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
  ==> "BuildSetup"

RunTargetOrDefault "Publish"
