using UnityEngine;

public class AssemblyMeshManager : MonoBehaviour
{
    public static AssemblyMeshManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ShowOnly(string meshName)
    {
        Debug.Log("ShowOnly called for: " + meshName);

        foreach (Transform child in transform)
        {
            bool match = child.name.Equals(meshName, System.StringComparison.OrdinalIgnoreCase);
            Debug.Log($"Child: {child.name}, Match: {match}");
            child.gameObject.SetActive(match);
        }
    }
}