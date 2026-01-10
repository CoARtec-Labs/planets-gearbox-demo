using System;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The view of the assembly state.
/// </summary>
public class AssemblyView10_Ring : BaseViewSingleton<AssemblyView10_Ring>
{
    // Events to attach to.
    public static UnityAction OnNextClicked;
    public static UnityAction OnBackClicked;
    public static UnityAction OnStagingClicked;

    // public GameObject InstructionsCanvas;
    
    public GameObject ring;

    private const string Title = 
        "Schritt 1: Gehäusunterteil";
    private const string Description = 
        "Legen Sie das Gehäuseunterteil mit den Zähnen nach oben in die Montagehalterung.";
    
    // public void Awake()
    // {
    //     base.Awake();
    //     
    //     // transform.GetComponentsInChildren<BaseView>(true);
    // }
    
    public void OnEnable()
    {
        // Debug.Log($"View step ring: OnEnable()");
        if (ring != null)
            ring.SetActive(true);
        
        InitializeMenu(Title, Description);
    }
    
    public void OnDisable()
    {
        // Debug.Log($"View step ring: OnDisable()");
        if (ring != null)
            ring.SetActive(false);
    }

    public void NextClick()
    {
        OnNextClicked?.Invoke();
    }

    public void BackClick()
    {
        OnBackClicked?.Invoke();
    }

    public void StagingClick()
    {
        OnStagingClicked?.Invoke();
    }
    
}
