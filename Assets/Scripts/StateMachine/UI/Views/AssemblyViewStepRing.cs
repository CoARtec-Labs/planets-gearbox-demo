using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The view of the assembly state.
/// </summary>
public class AssemblyViewStepRing : BaseViewSingleton<AssemblyViewStepRing>
{
    public static UnityAction OnNextClicked;
    public static UnityAction OnBackClicked;

    // You can optionally reference or hardcode the mesh name
    private readonly string meshName = "ring";

    public override void ShowView()
    {
        base.ShowView();
        Debug.Log("Ring View Loaded - Requesting Mesh Visibility");
        AssemblyMeshManager.Instance.ShowOnly("ring");
    }

    public void NextClick()
    {
        OnNextClicked?.Invoke();
    }

    public void BackClick()
    {
        OnBackClicked?.Invoke();
    }
}