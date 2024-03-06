# Step1: Export Project:

In Unity: File -> Build Settings...:

Build oder Export Project zu einem Ordner zb: androidBuild/

# Step2: Build Ordner löschen:

Folgende Ordner löschen (Im übergeordneten Ordner des Android Projektes):

AndroidARPlanetengetriebeUnityBuild/unityLibrary/build
AndroidARPlanetengetriebeUnityBuild/unityLibrary/xrmanifest.androidlib/build

# Step3: libs und symbols Ordner ersetzen:

Folgende Ordner aus dem neuen android build Ordner: androidBuild/unityLibrary/ kopieren oder verschieben:

libs/
symbols/

und in den Ordner einfügen:

AndroidARPlanetengetriebeUnityBuild/unityLibrary/ 

# Step4: src/main Ordner ersetzen:

Folgende Ordner aus dem neuen android build Ordner androidBuild/unityLibrary/src/main/ kopieren oder verschieben:

assets/
Il2CppOutputProject/
jniLibs/
jniStaticLibs/
resources/

und in den Ordner einfügen:

AndroidARPlanetengetriebeUnityBuild/unityLibrary/src/main/ 

# Step5: Rebuild Android Project:

In Androidtudio: 
Build -> Rebuild Project

oder Run 'app'


# Automatisierungsansatz:

https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Android.IPostGenerateGradleAndroidProject.html

Implementiere das Interface IPostGenerateGradleAndroidProject und platziere es in den Ordner 'Assets/Editor'. 
In der OnPostGenerateGradleAndroidProject Funktion: Editiere die Gradle und Manifest Dateien, um sie mit der aktuellen Android Project version kompatibel zu halten

Beispiele:

using UnityEditor;
using UnityEditor.Android;
using UnityEngine;

class AndroidBuildProcessor : IPostGenerateGradleAndroidProject
{
    public int callbackOrder { get { return 0; } }
    public void OnPostGenerateGradleAndroidProject(string path)
    {
        Debug.Log("AndroidBuildProcessor.OnPostGenerateGradleAndroidProject at path " + path);
    }
}

using System.IO;
using UnityEditor;
using UnityEditor.Android;
using UnityEngine;

/// <summary>
/// Used as a workaround for Unity 2022.x who isn't able to set the installlocation value to "auto" in the AndroidManifest.xml file.
/// Should be put in the Assets/Editor folder, otherwise you'll get errors on build.
/// The issue can be followed here : https://issuetracker.unity3d.com/issues/android-install-location-changes-when-exporting-project
/// </summary>
class CustomGradleProcessor : IPostGenerateGradleAndroidProject
{
    public int callbackOrder { get { return 0; } }
    public void OnPostGenerateGradleAndroidProject(string path)
    {
        Debug.Log("CustomGradleProcessor.OnPostGenerateGradleAndroidProject at path " + path);

        // Dirty hack to get the path to the AndroidManifest.xml file
        path = path.Replace("unityLibrary", "launcher\\src\\main\\AndroidManifest.xml");

        // Read the AndroidManifest.xml file and replace the preferExternal value
        string text = File.ReadAllText(path);
        text = text.Replace("preferExternal", "auto");

        // Write it all back
        File.WriteAllText(path, text);

        Debug.Log($"preferExternal replaced at {path}");
    }
}
