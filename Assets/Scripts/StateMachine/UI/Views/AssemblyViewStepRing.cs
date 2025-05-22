using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The view of the assembly state.
/// </summary>
public class AssemblyViewStepRing : BaseViewSingleton<AssemblyViewStepRing>
{
    // Events to attach to.
    public static UnityAction OnNextClicked;
    public static UnityAction OnBackClicked;
    public static UnityAction OnStagingClicked;

    // public GameObject InstructionsCanvas;
    
    public GameObject ring;
    
    public void OnEnable()
    {
        Debug.Log($"View step ring: OnEnable()");
        if (ring != null)
            ring.SetActive(true);
    }
    public void OnDisable()
    {
        Debug.Log($"View step ring: OnDisable()");
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
