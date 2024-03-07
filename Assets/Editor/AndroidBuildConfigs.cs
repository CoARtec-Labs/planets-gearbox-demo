using System;
using System.Collections.Generic;
using AndroidBuildExtensions;

namespace AndroidBuildConfigurations
{
    static class AndroidBuildConfiguration
    {
        internal const string PACKAGE_NAME = "com.CoARTec.AR_Planetengetriebe_v1";
        private const string XRMANIFEST_PACKAGE_NAME = "com.UnityTechnologies.XR.Manifest";

        internal static List<string> directoriesToDelete = new()
        {
            "unityLibrary/build",
            "unityLibrary/xrmanifest.androidlib/build",
            "unityLibrary/src/main/java/com/unity3d"
        };
        internal static List<string> styleFilesPath =new()
            {
                "unityLibrary/src/main/res/values/styles.xml",
                "unityLibrary/src/main/res/values-v21/styles.xml",
                "unityLibrary/src/main/res/values-v28/styles.xml",
                "unityLibrary/src/main/res/values-v31/styles.xml"
            };


        internal static Dictionary<string, string> launcherManifestTagValues = new()
        {
            {"android:installLocation", "auto"}
        };

        internal static Dictionary<string, string> unityLibraryManifestTagValues = new()
        {
            {"android:hardwareAccelerated", "true"},
            {"android:exported", "false"}
        };

        internal static List<string> unityLibraryDependencies = new(){
            "\"androidx.appcompat:appcompat:$appcompatVersion\"",
            "\"androidx.core:core-ktx:$coreKtxVersion\"",
            "\"androidx.activity:activity-compose:$activityComposeVersion\"",
            "platform(\"androidx.compose:compose-bom:$composeBomVersion\")",
            "'androidx.compose.ui:ui'",
            "'androidx.compose.ui:ui-graphics'",
            "'androidx.compose.ui:ui-tooling-preview'",
            "'androidx.compose.material3:material3'",
            "\"com.jakewharton.timber:timber:$timberVersion\""
        };

        internal static List<string> buildGradleDeleteKeyWords = new(){
                                "apply plugin",
                                "buildToolsVersion",
                                "ndkPath",
                                "compileSdkVersion"

                            };

        internal static List<string> unityLibraryPlugins = new()
        {
            "'com.android.library'",
            "'org.jetbrains.kotlin.android'"
        };

        internal static List<string> xrManifestLibraryPlugins = new() 
        {
            "'com.android.library'"

        };

        internal static List<KeyValuePair<string, string>> commonAndroidConfigs = new()
                    {
                        new("namespace", $"'{PACKAGE_NAME}'"),
                        new("compileSdk", "compileSdkVersionCode")
                    };

        internal static List<KeyValuePair<string, string>> commonAndroidDefaultConfigs = new()
                    {
                        new("targetSdkVersion", "targetSdkVersionCode"),
                        new("minSdkVersion", "minSdkVersionCode")
                    };

        internal static Dictionary<string, List<KeyValuePair<string, string>>> commonConfigs = new()
        {
            {
                    "android", 
                    commonAndroidConfigs
                },

                {
                    "android.defaultConfig", 
                    commonAndroidDefaultConfigs
                    
                }

        };

        internal static Dictionary<string, List<KeyValuePair<string, string>>> UnityLibraryConfigs {
            get
            {
                List<KeyValuePair<string, string>> defaultConfig = new(commonAndroidDefaultConfigs)
                {
                    new("ndkVersion", "'23.1.7779620'")
                };

                Dictionary<string, List<KeyValuePair<string, string>>> configs = new(commonConfigs)
                {
                    ["android.defaultConfig"] = defaultConfig,
                    ["android.kotlinOptions"] = new List<KeyValuePair<string, string>>()
                    {
                        new("jvmTarget", "= '11'"),
                    },
                    ["android.buildFeatures"] = new List<KeyValuePair<string, string>>()
                    {
                        new("compose", "true"),
                    },
                    ["android.composeOptions"] = new List<KeyValuePair<string, string>>()
                    {
                        new("kotlinCompilerExtensionVersion", "'1.5.3'"),
                    }
                };

                return  configs;
            }
        }
        internal static Dictionary<string, List<KeyValuePair<string, string>>> XrManifestLibraryConfigs {
            get
            {
                List<KeyValuePair<string, string>> androidConfigs = new(commonAndroidConfigs)
                {
                    [0] = new("namespace", $"'{XRMANIFEST_PACKAGE_NAME}'")
                };
                Dictionary<string, List<KeyValuePair<string, string>>> configs = new(commonConfigs)
                {
                    ["android"] = androidConfigs
                };

                return  configs;
            }
        }
    }

        


    static class AndroidBuildTasks
    {
        internal static string CommonManifestTasks(string text)
            {
                return text
                    .DeletePackageInfo()// Delete 'package="..." '  from manifest file
                    .ReplaceUnityPackageName(AndroidBuildConfiguration.PACKAGE_NAME);
            }
        private static readonly Dictionary<string, Func<string, string>> manifestFilesTasks = new()
        {
            {
                "launcher/src/main/AndroidManifest.xml", 
                (text) => text.EditXmlTagValues(AndroidBuildConfiguration.launcherManifestTagValues)
            }, 
            {
                "unityLibrary/src/main/AndroidManifest.xml", 
                (text) => text
                        .EditXmlTagValues(AndroidBuildConfiguration.unityLibraryManifestTagValues)
                        .DeleteIntentFilter()
            },
            {
                "unityLibrary/xrmanifest.androidlib/AndroidManifest.xml", 
                (text) => text
            }
        };

        internal static Dictionary<string, Func<string, string>> ManifestFilesTasks
        {
            get => manifestFilesTasks;
        }


        internal static LinkedList<string> CommonBuildGradleTasks(string text)
        {
            return text
                    .ToLinkedList()
                    .FilterOutObsoleteInfos(AndroidBuildConfiguration.buildGradleDeleteKeyWords);
                
        }
        private static readonly Dictionary<string, Func<LinkedList<string>, string>> buildGradleFileTasks = new()
        {
            {
                "unityLibrary/build.gradle", 
                (list) => list
                        .AddPlugins(AndroidBuildConfiguration.unityLibraryPlugins)
                        .AddDependencies(AndroidBuildConfiguration.unityLibraryDependencies)
                        .AddAndroidConfiguration(AndroidBuildConfiguration.UnityLibraryConfigs)
                        .ToText()
            },
            {
                "unityLibrary/xrmanifest.androidlib/build.gradle",
                (list) => list
                        .AddPlugins(AndroidBuildConfiguration.xrManifestLibraryPlugins)
                        .AddAndroidConfiguration(AndroidBuildConfiguration.XrManifestLibraryConfigs)
                        .ToText()

            }
        };

        internal static Dictionary<string, Func<LinkedList<string>, string>> BuildGradleFileTasks
        {
            get => buildGradleFileTasks;
        }
        internal static string CommonStylesTasks(string text)
            {
                return text.EditStylesTheme();
                    
            }

        private static Dictionary<string, Func<string, string>> stylesTasks ;

        internal static Dictionary<string, Func<string, string>> StylesTasks
        {
            get 
            {
                if (stylesTasks == null )
                {
                    stylesTasks = new Dictionary<string, Func<string, string>>();
                }
                AndroidBuildConfiguration.styleFilesPath.ForEach((path)=>
                {
                    stylesTasks.TryAdd(path, (text) => text);
                });
                return stylesTasks;
            }
        }
        
    }
}