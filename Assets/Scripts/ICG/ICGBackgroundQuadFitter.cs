using UnityEngine;

namespace ICG
{
    public class ICGBackgroundQuadFitter : MonoBehaviour
    {
        public Camera cam;                 // OverlayCamera
        public Transform quad;             // RGB_Quad Transform
        public float distance = 1.0f;      // Abstand vor der Kamera

        [Header("Image Orientation")]
        [Tooltip("Muss zum ICGRgbPreview.flipY passen. Wenn das RGB-Bild vertikal geflippt dargestellt wird, muss auch das Principal-Point/Frustum-Mapping geflippt werden, sonst passt die Projektion nicht.")]
        public bool flipY = true;

        [Tooltip("Falls du einen konstanten Off-by-~0.5px siehst, hier toggeln. Viele Pipelines nutzen Pixel-Center-Koordinaten (u+0.5, v+0.5).")]
        public bool usePixelCenterOffset = true;

        private bool _applied = false;

        void LateUpdate()
        {
            if (_applied) return;
            if (cam == null || quad == null) return;

            if (!ICGNative.TryGetColorIntrinsics(out float fx, out float fy, out float cx, out float cy, out int w, out int h))
                return;

            if (usePixelCenterOffset)
            {
                cx = cx + 0.5f;
                cy = cy + 0.5f;
            }

            // Wenn das Bild vertikal geflippt dargestellt wird: cy spiegeln
            // (OpenCV cy ist von oben gemessen; Flip ändert die Bildkoordinaten.)
            if (flipY)
            {
                cy = (h - cy);
            }

            // Frustum-Kanten (in Kamera-Koordinaten bei z = distance)
            float left   = -cx / fx * distance;
            float right  =  (w - cx) / fx * distance;
            float top    =  cy / fy * distance;
            float bottom = -(h - cy) / fy * distance;

            float width  = right - left;
            float height = top - bottom;
            float centerX = (left + right) * 0.5f;
            float centerY = (top + bottom) * 0.5f;

            // Quad sitzt vor der Kamera und füllt das Bild exakt (inkl. Principal-Point-Offset)
            quad.localPosition = new Vector3(centerX, centerY, distance);
            quad.localRotation = Quaternion.identity;
            quad.localScale = new Vector3(width, height, 1f);

            _applied = true;
            Debug.Log($"[ICG] Quad fitted: size=({width:F3},{height:F3}) center=({centerX:F3},{centerY:F3}) z={distance} flipY={flipY}");
        }
    }
}
