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
