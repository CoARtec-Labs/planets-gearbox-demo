using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Android;
using AndroidBuildExtensions;
using AndroidBuildConfigurations;
class AndroidBuildProcessor : IPostGenerateGradleAndroidProject
{
    public int callbackOrder { get { return 99; } }
    

    public void OnPostGenerateGradleAndroidProject(string path)
    {
        // Debug.Log("AndroidBuildProcessor.OnPostGenerateGradleAndroidBuildConfiguration at path " + path);

        DeleteDirectories(path);
        ModifyManifestFiles(path);
        ModifyBuildGradleFiles(path);
        ModifyStylesXmlFiles(path);
    }

    private void DeleteDirectories(string rootProject)
    {
    
        AndroidBuildConfiguration.directoriesToDelete.ForEach((path) =>
        {
            var fullPath = rootProject.GetPathToFileOrDir(path);
            if (Directory.Exists(fullPath))
            {
                DirectoryInfo dir = new(fullPath);
                dir.Delete(true);
            }
        });

    }

    
    private void ModifyFile(string rootProject, string relativeFilePath, Func<string, string> operation)
    {
        string path = rootProject.GetPathToFileOrDir(relativeFilePath);
        
        string text = File.ReadAllText(path);
        text = operation(text);
        // Debug.Log($"ModifyFile: text={text}");
        File.WriteAllText(path, text);

        // Debug.Log($"ModifyFile: File {path} Modified");

    }

    private void ModifyManifestFiles(string rootProject)
    {
        

        ModifyAndroidFiles(rootProject, AndroidBuildTasks.ManifestFilesTasks, AndroidBuildTasks.CommonManifestTasks);
        
    }

    private void ModifyBuildGradleFiles(string rootProject)
    {
        
        ModifyAndroidFiles(rootProject, AndroidBuildTasks.BuildGradleFileTasks, AndroidBuildTasks.CommonBuildGradleTasks);

    }

    private void ModifyAndroidFiles<T>(string rootProject, Dictionary<string, Func<T, string>> tasks, Func<string, T> commonTasks)
    {
        foreach(var task in tasks)
        {
            ModifyFile(
                rootProject,
                task.Key,
                (text) => {
                    T commonTasksResult = commonTasks(text);
                    return task.Value(commonTasksResult);
                }
                );
        }
    }

    private void ModifyStylesXmlFiles(string rootProject)
    {
        
        ModifyAndroidFiles(rootProject, AndroidBuildTasks.StylesTasks, AndroidBuildTasks.CommonStylesTasks);
    }
    
}


