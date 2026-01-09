using UnityEngine;

[ExecuteAlways]
public class ICGCenterMeshByBounds : MonoBehaviour
{
    public Vector3 extraOffsetMm = Vector3.zero; // Feintuning in mm

    void OnEnable() => Apply();
    void Start() => Apply();
#if UNITY_EDITOR
    void Update() { if (!Application.isPlaying) Apply(); }
#endif

    void Apply()
    {
        var mf = GetComponent<MeshFilter>();
        if (!mf || !mf.sharedMesh) return;

        // bounds.center ist in Mesh-Einheiten (bei OBJ i.d.R. mm)
        Vector3 c = mf.sharedMesh.bounds.center;

        // Wir verschieben das Mesh so, dass sein Zentrum bei (0,0,0) liegt
        transform.localPosition = -(c + extraOffsetMm);
    }
}
