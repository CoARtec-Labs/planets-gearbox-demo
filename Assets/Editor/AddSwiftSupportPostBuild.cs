using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public static class AddSwiftSupportPostBuild
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget != BuildTarget.iOS)
            return;

        string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(projPath);

#if UNITY_2019_3_OR_NEWER
        string appTarget = proj.GetUnityMainTargetGuid();
        string frameworkTarget = proj.GetUnityFrameworkTargetGuid();
#else
        string appTarget = proj.TargetGuidByName("Unity-iPhone");
        string frameworkTarget = appTarget;
#endif

        // Add Dummy.swift
        string swiftFileRelative = "Classes/Dummy.swift";
        string swiftFilePath = Path.Combine(pathToBuiltProject, swiftFileRelative);
        if (!File.Exists(swiftFilePath))
        {
            File.WriteAllText(swiftFilePath,
                "// Dummy Swift file to force Swift runtime inclusion\n" +
                "import Foundation\n" +
                "class DummySwift {}\n");
        }

        proj.AddFile(swiftFileRelative, swiftFileRelative, PBXSourceTree.Source);
        proj.AddFileToBuild(swiftFileRelative, appTarget);

        // Apply to both targets
        foreach (string target in new[] { appTarget, frameworkTarget })
        {
            proj.SetBuildProperty(target, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
            proj.SetBuildProperty(target, "SWIFT_VERSION", "5.0");
            proj.SetBuildProperty(target, "IPHONEOS_DEPLOYMENT_TARGET", "14.0");
        }

        proj.WriteToFile(projPath);
    }
}