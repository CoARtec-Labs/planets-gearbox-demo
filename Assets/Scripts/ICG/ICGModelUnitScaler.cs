using UnityEngine;

namespace ICG
{
    /// <summary>
    /// Hilfsscript: skaliert ein Mesh-Root in "Meter".
    /// Für ICG-Beispiele ist geometry_unit_in_meter oft 0.001 (OBJ in mm).
    /// </summary>
    public class ICGModelUnitScaler : MonoBehaviour
    {
        [Tooltip("Skalierung: 0.001 = OBJ ist in Millimeter, 1.0 = bereits Meter")]
        public float unitInMeter = 0.001f;

        [Tooltip("Optionaler zusätzlicher Faktor")]
        public float extraScale = 1.0f;

        void Reset() => Apply();
        void OnValidate() => Apply();

        private void Apply()
        {
            float s = unitInMeter * extraScale;
            transform.localScale = new Vector3(s, s, s);
        }
    }
}
