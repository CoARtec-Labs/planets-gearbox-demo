using TMPro;
using UnityEngine;

/// <summary>
/// Template class providing the singleton pattern and (de)activation for UI views.
/// </summary>
public class BaseViewSingleton<T> : BaseView where T : Component
{
    public static T Instance {get; private set;}

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;

            // This one only works on root GameObjects. It is not
            // really necessary for our View objects anyways.
            //DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    protected void InitializeMenu(string titleText, string descriptionText)
    {
        Transform title = transform.Find("AssemblyMenu/Canvas/Title");
        Transform description = transform.Find("AssemblyMenu/Canvas/Description");

        // Transform title = menu.gameObject.GetChildGameObjects("Title");

        title.GetComponent<TMP_Text>().SetText(titleText);
        description.GetComponent<TMP_Text>().SetText(descriptionText);
    }
}
