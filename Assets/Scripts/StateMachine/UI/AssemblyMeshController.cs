// AssemblyMeshController.cs
using UnityEngine;

public class AssemblyMeshController : MonoBehaviour
{
    // Assign these in Inspector (drag child meshes of Origin)
    [Header("Meshes")]
    public GameObject ring;
    public GameObject sun;
    public GameObject planet1;
    public GameObject planet2;
    public GameObject planet3;
    public GameObject carrier;
    public GameObject gasket;
    public GameObject lid;

    // Hide all meshes by default
    private void Awake()
    {
        HideAll();
    }

    // Hide all meshes
    public void HideAll()
    {
        ring.SetActive(false);
        sun.SetActive(false);
        planet1.SetActive(false);
        planet2.SetActive(false);
        planet3.SetActive(false);
        carrier.SetActive(false);
        gasket.SetActive(false);
        lid.SetActive(false);
    }

    // Show only one mesh (call this from states)
    public void ShowOnly(GameObject targetMesh)
    {
        Debug.Log($"Showing mesh: {targetMesh?.name}");
        HideAll();
        if (targetMesh != null) 
        {
            targetMesh.SetActive(true);
            Debug.Log($"Mesh active state: {targetMesh.activeSelf}");
        }
    }
}