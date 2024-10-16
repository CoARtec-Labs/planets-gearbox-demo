using UnityEngine;

/// <summary>
/// UI Root class, used for storing references to UI views.
/// </summary>
public class UIRootStaging : MonoBehaviour
{
    [SerializeField]
    private StagingView stagingView;
    public StagingView StagingView => stagingView;


    public void Awake()
    {
        Debug.Log("[UIRootStaging.cs] Awake ...");

        StagingView.gameObject.SetActive(true);

        StagingView.Instance.HideView();
    }

}
