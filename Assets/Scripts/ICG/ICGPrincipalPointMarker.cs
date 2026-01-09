using UnityEngine;

namespace ICG
{
    public class ICGPrincipalPointMarker : MonoBehaviour
    {
        public Transform backgroundQuad;   // RGB_Quad
        public Transform marker;           // PP_Marker

        [Tooltip("Muss zu ICGRgbPreview.flipY passen.")]
        public bool flipY = true;

        [Tooltip("Pixel-Center Offset (u+0.5, v+0.5) — falls nötig.")]
        public bool usePixelCenterOffset = true;

        void LateUpdate()
        {
            if (backgroundQuad == null || marker == null) return;
            if (!ICGNative.TryGetColorIntrinsics(out _, out _, out float cx, out float cy, out int w, out int h))
                return;

            if (usePixelCenterOffset)
            {
                cx += 0.5f;
                cy += 0.5f;
            }

            if (flipY)
            {
                cy = h - cy;
            }

            // cx,cy (Pixel) -> normalized [-0.5..+0.5] mit Ursprung Bildmitte
            float nx = (cx / w) - 0.5f;
            float ny = 0.5f - (cy / h); // y nach oben

            // Quad localScale ist (width,height,1) in world units (weil fitted)
            Vector3 s = backgroundQuad.localScale;
            float x = nx * s.x;
            float y = ny * s.y;

            marker.localPosition = new Vector3(x, y, -0.001f);
        }
    }
}
