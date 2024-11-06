using UnityEngine;

/// <summary>
/// Template class providing the singleton pattern and (de)activation for UI views.
/// </summary>
public class BaseViewSingleton<T> : MonoBehaviour where T : Component
{
    public static T Instance {get; private set;}

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;

            // This one only works on root GameObjects. 
            // It is not really necessary for our View objects anyways.
            //DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// Base method called to show view
    /// </summary>
    public virtual void ShowView()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Base method called to hide view
    /// </summary>
    public virtual void HideView()
    {
        gameObject.SetActive(false);
    }
}
