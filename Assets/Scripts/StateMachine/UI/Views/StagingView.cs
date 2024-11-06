using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The StagingView class.
/// This classe is used to (de)activate the staging UI.
/// </summary>
public class StagingView : BaseViewSingleton<StagingView>
{
    // Events to attach to.
    public static UnityAction OnAssemblyClicked;

    public List<GameObject> ActiveGameObjects;

    /// <summary>
    /// Method for assembly button.
    /// </summary>
    public void AssemblyClick()
    {
        OnAssemblyClicked?.Invoke();
    }


    /// <summary>
    /// Specialized method called to activate all game objects.
    /// </summary>
    public override void ShowView()
    {
        //Debug.Log("[StagingView] show view.");
        // Show all objects
        //GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in ActiveGameObjects)
        {
            // Debug.Log($"Activate {obj.name}");
            obj.SetActive(true);
        }
    }

    /// <summary>
    /// Specialized method called to deactivate all but this game objects.
    /// </summary>
    public override void HideView()
    {
        // Hide all objects
        // GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in ActiveGameObjects)
        {
            // Debug.Log($"Deactivate {obj.name}");
            obj.SetActive(false);
        }

        // foreach (GameObject obj in allObjects)
        // {
        //     if(obj.name == gameObject.name || obj.name == gameObject.transform.parent.name)
        //     // if(GameObject.ReferenceEquals(obj, gameObject) ||
        //     //     GameObject.ReferenceEquals(obj, gameObject.transform.parent))
        //     {}
        //     else
        //     {
        //         obj.SetActive(false);
        //     }
        // }
    }

}
