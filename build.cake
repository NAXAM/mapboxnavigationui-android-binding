#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0

// Cake Addins
#addin nuget:?package=Cake.FileHelpers&version=2.0.0

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var VERSION = "4.8.0";
var NUGET_SUFIX = ".1";
var NAV_VERSION = "0.11.1";

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var artifacts = new [] {
    // new Artifact {
    //     Version = VERSION + NUGET_SUFIX,
    //     NativeVersion = VERSION,
    //     ReleaseNotes = new string [] {
    //         "Mapbox for Android - SdkCore v{0}"
    //     },
    //     SolutionPath = "./mapboxsdkcore-droid/mapboxsdkcore-droid.sln",
    //     AssemblyInfoPath = "./mapboxsdkcore-droid/Naxam.MapboxSdkCore.Droid/Properties/AssemblyInfo.cs",
    //     NuspecPath = "./mapboxsdkcore-droid/mapboxsdkcore.nuspec",
    //     DownloadUrl = "http://central.maven.org/maven2/com/mapbox/mapboxsdk/mapbox-sdk-core/{0}/mapbox-sdk-core-{0}.jar",
    //     JarPath = "./mapboxsdkcore-droid/Naxam.MapboxSdkCore.Droid/Jars/mapbox-sdk-core.jar",
    //     Dependencies = new NuSpecDependency[] {
    //             new NuSpecDependency {
    //                 Id = "GoogleGson",
    //                 Version = "2.8.5"
    //             },
    //             new NuSpecDependency {
    //                 Id = "Naxam.Retrofit2.ConvertGson.Droid",
    //                 Version = "2.4.0"
    //             },
                  
    //             new NuSpecDependency {
    //                 Id = "Naxam.SquareUp.OkHttp3.LoggingInterceptor",
    //                 Version = "3.11.0"
    //             },
    //             new NuSpecDependency {
    //                 Id = "Square.Retrofit2",
    //                 Version = "2.3.0"
    //             }

    //     }
    // },
    new Artifact {
        Version = VERSION + NUGET_SUFIX,
        NativeVersion = VERSION,
        ReleaseNotes = new string [] {
            "Mapbox for Android - SdkGeoJSON v{0}"
        },
        SolutionPath = "./mapboxsdkgeojson-droid/MapboxSdkGeojson-Droid.sln",
        AssemblyInfoPath = "./mapboxsdkgeojson-droid/Naxam.MapboxSdkGeojson.Droid/Properties/AssemblyInfo.cs",
        NuspecPath = "./mapboxsdkgeojson-droid/mapboxsdkgeojson.nuspec",
        DownloadUrl = "http://central.maven.org/maven2/com/mapbox/mapboxsdk/mapbox-sdk-geojson/{0}/mapbox-sdk-geojson-{0}.jar",
        JarPath = "./mapboxsdkgeojson-droid/Naxam.MapboxSdkGeojson.Droid/Jars/mapbox-sdk-geojson.jar",
        Dependencies = new NuSpecDependency[] {
            // new NuSpecDependency {
            //     Id = "Naxam.MapboxSdkCore.Droid",
            //     Version = VERSION + NUGET_SUFIX
            // }
        }
    },
    // new Artifact {
    //     Version = VERSION + NUGET_SUFIX,
    //     NativeVersion = VERSION,
    //     ReleaseNotes = new string [] {
    //         "Mapbox for Android - SdkTurf v{0}"
    //     },
    //     SolutionPath = "./mapboxsdkturf-droid/mapboxsdkturf-droid.sln",
    //     AssemblyInfoPath = "./mapboxsdkturf-droid/Naxam.MapboxSdkTurf.Droid/Properties/AssemblyInfo.cs",
    //     NuspecPath = "./mapboxsdkturf-droid/mapboxsdkturf.nuspec",
    //     DownloadUrl = "http://central.maven.org/maven2/com/mapbox/mapboxsdk/mapbox-sdk-turf/{0}/mapbox-sdk-turf-{0}.jar",
    //     JarPath = "./mapboxsdkturf-droid/Naxam.MapboxSdkTurf.Droid/Jars/mapbox-sdk-turf.jar",
    //     Dependencies = new NuSpecDependency[] {
    //         new NuSpecDependency {
    //             Id = "Naxam.MapboxSdkGeojson.Droid",
    //             Version = VERSION + NUGET_SUFIX
    //         }
    //     }
    // },
    // new Artifact {
    //     Version = VERSION + NUGET_SUFIX,
    //     NativeVersion = VERSION,
    //     ReleaseNotes = new string [] {
    //         "Mapbox for Android - SdkService v{0}"
    //     },
    //     SolutionPath = "./mapboxsdkservice-droid/mapboxsdkservice-droid.sln",
    //     AssemblyInfoPath = "./mapboxsdkservice-droid/Naxam.MapboxSdkServices.Droid/Properties/AssemblyInfo.cs",
    //     NuspecPath = "./mapboxsdkservice-droid/mapboxsdkservice.nuspec",
    //     DownloadUrl = "http://central.maven.org/maven2/com/mapbox/mapboxsdk/mapbox-sdk-services/{0}/mapbox-sdk-services-{0}.jar",
    //     JarPath = "./mapboxsdkservice-droid/Naxam.MapboxSdkServices.Droid/Jars/mapbox-sdk-services.jar",
    //     Dependencies = new NuSpecDependency[] {
    //         new NuSpecDependency {
    //             Id = "Naxam.MapboxSdkGeojson.Droid",
    //             Version = VERSION + NUGET_SUFIX
    //         }
    //     }
    // },
    // new Artifact {
    //     Version = NAV_VERSION,
    //     NativeVersion = NAV_VERSION,
    //     ReleaseNotes = new string [] {
    //         "Mapbox Navigation for Android v{0}"
    //     },
    //     SolutionPath = "./mapboxnavigation-droid/mapboxnavigation-droid.sln",
    //     AssemblyInfoPath = "./mapboxnavigation-droid/Naxam.MapboxNavigation.Droid/Properties/AssemblyInfo.cs",
    //     NuspecPath = "./mapboxnavigation-droid/lib.nuspec",
    //     DownloadUrl = "http://central.maven.org/maven2/com/mapbox/mapboxsdk/mapbox-android-navigation/{0}/mapbox-android-navigation-{0}.aar",
    //     JarPath = "./mapboxnavigation-droid/Naxam.MapboxNavigation.Droid/Jars/mapbox-android-navigation.aar",
    //     Dependencies = new NuSpecDependency[] {
    //         new NuSpecDependency {
    //             Id = "Naxam.MapboxSdkServices.Droid",
    //             Version = VERSION + NUGET_SUFIX
    //         },
    //         new NuSpecDependency {
    //             Id = "Naxam.MapboxSdkTurf.Droid",
    //             Version = VERSION + NUGET_SUFIX
    //         },
    //         new NuSpecDependency {
    //             Id = "Naxam.Mapbox.Services.Droid",
    //             Version = "2.2.9.1"
    //         },
    //     },
    // },
    // new Artifact {
    //     Version = NAV_VERSION + ".1",
    //     NativeVersion = NAV_VERSION,
    //     ReleaseNotes = new string [] {
    //         "Mapbox NavigationUI for Android v{0}"
    //     },
    //     SolutionPath = "./mapboxnavigationui-droid/mapboxnavigationui-droid.sln",
    //     AssemblyInfoPath = "./mapboxnavigationui-droid/Naxam.MapboxNavigationUI.Droid/Properties/AssemblyInfo.cs",
    //     NuspecPath = "./mapboxnavigationui-droid/lib.nuspec",
    //     DownloadUrl = "http://central.maven.org/maven2/com/mapbox/mapboxsdk/mapbox-android-navigation-ui/{0}/mapbox-android-navigation-ui-{0}.aar",
    //     JarPath = "./mapboxnavigationui-droid/Naxam.MapboxNavigationUI.Droid/Jars/mapbox-android-navigation-ui.aar",
    //     Dependencies = new NuSpecDependency[] {
    //         new NuSpecDependency {
    //             Id = "Naxam.Mapbox.Droid",
    //             Version = "5.5.1.1"
    //         },
    //         new NuSpecDependency {
    //             Id = "Naxam.MapboxNavigation.Droid",
    //             Version = NAV_VERSION
    //         },
    //         new NuSpecDependency {
    //             Id = "Square.Picasso",
    //             Version = "2.5.2.1"
    //         },
    //         new NuSpecDependency {
    //             Id = "Xamarin.Android.Arch.Core.Runtime",
    //             Version = "1.0.0"
    //         },
    //         new NuSpecDependency {
    //             Id = "Xamarin.Android.Arch.Lifecycle.Extensions",
    //             Version = "1.0.0"
    //         },
    //         new NuSpecDependency {
    //             Id = "Xamarin.Android.Support.v7.CardView",
    //             Version = "26.1.0.1"
    //         },
    //         new NuSpecDependency {
    //             Id = "Xamarin.Android.Support.v7.RecyclerView",
    //             Version = "26.1.0.1"
    //         },
    //         new NuSpecDependency {
    //             Id = "Xamarin.Android.Support.Design",
    //             Version = "26.1.0.1"
    //         },
    //         new NuSpecDependency {
    //             Id = "Xamarin.Android.Support.v4",
    //             Version = "26.1.0.1"
    //         },
    //         new NuSpecDependency {
    //             Id = "Xamarin.Android.Support.Constraint.Layout",
    //             Version = "1.0.2.2"
    //         },
    //         new NuSpecDependency {
    //             Id = "Xbindings.AWSSdkPolly.Droid",
    //             Version = "2.3.8"
    //         },
    //         new NuSpecDependency {
    //             Id="Xbindings.MapboxLocationLayer.Droid",
    //             Version="0.4.0.2"                
    //         }
    //     }
    // }
};

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Downloads")
    .Does(() =>
{
    foreach(var artifact in artifacts) {
        var downloadUrl = string.Format(artifact.DownloadUrl, artifact.NativeVersion);
        var jarPath = string.Format(artifact.JarPath, artifact.NativeVersion);

        DownloadFile(downloadUrl, jarPath);
    }
});

Task("Clean")
    .Does(() =>
{
    CleanDirectory("**/*/packages");

    CleanDirectory("./nugets/*");

    var nugetPackages = GetFiles("./nugets/*.nupkg");

    foreach (var package in nugetPackages)
    {
        DeleteFile(package);
    }
});

Task("UpdateVersion")
    .Does(() => 
{
    foreach(var artifact in artifacts) {
        ReplaceRegexInFiles(artifact.AssemblyInfoPath, "\\[assembly\\: AssemblyVersion([^\\]]+)\\]", string.Format("[assembly: AssemblyVersion(\"{0}\")]", artifact.Version));
    }
});

Task("Pack")
    .IsDependentOn("Downloads")
    .IsDependentOn("UpdateVersion")
    .Does(() =>
{
    foreach(var artifact in artifacts) {
        NuGetRestore(artifact.SolutionPath);
        MSBuild(artifact.SolutionPath, settings => {
            settings.ToolVersion = MSBuildToolVersion.VS2019;
            settings.SetConfiguration(configuration);
        });
        NuGetPack(artifact.NuspecPath, new NuGetPackSettings {
            Version = artifact.Version,
            Dependencies = artifact.Dependencies,
            ReleaseNotes = artifact.ReleaseNotes,
            OutputDirectory = "./nugets"
        });
    }
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Pack");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

class Artifact {
    public string Version { get; set; }
    public string NativeVersion { get; set; }

    public string AssemblyInfoPath { get; set; }

    public string SolutionPath { get; set; }

    public string DownloadUrl  { get; set; }

    public string JarPath { get; set; }

    public string NuspecPath { get; set; }

    public string[] ReleaseNotes { get; set; }

    public NuSpecDependency[] Dependencies { get; set; }
}