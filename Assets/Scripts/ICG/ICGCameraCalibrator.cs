using UnityEngine;

namespace ICG
{
    public class ICGCameraCalibrator : MonoBehaviour
    {
        public Camera targetCamera;

        [Header("Image Orientation")]
        [Tooltip("Muss zum ICGRgbPreview.flipY passen. Wenn das RGB-Bild vertikal geflippt dargestellt wird, muss auch die Projektion entsprechend geflippt werden.")]
        public bool flipY = true;

        [Tooltip("Falls du einen konstanten Off-by-~0.5px siehst, hier toggeln. Viele Pipelines nutzen Pixel-Center-Koordinaten (u+0.5, v+0.5).")]
        public bool usePixelCenterOffset = true;

        private bool _applied = false;

        void Update()
        {
            if (_applied) return;

            if (targetCamera == null)
            {
                Debug.LogWarning("[ICG] Calibrator: targetCamera not set.");
                return;
            }

            if (!ICGNative.TryGetColorIntrinsics(out float fx, out float fy, out float cx, out float cy, out int w, out int h))
                return; // noch nicht verfügbar → später nochmal

            if (usePixelCenterOffset)
            {
                cx = cx + 0.5f;
                cy = cy + 0.5f;
            }

            if (flipY)
            {
                cy = h - cy;
            }

            float near = Mathf.Max(0.01f, targetCamera.nearClipPlane);
            float far  = Mathf.Max(near + 0.01f, targetCamera.farClipPlane);

            float left   = -cx / fx * near;
            float right  = (w - cx) / fx * near;
            float top    =  cy / fy * near;
            float bottom = -(h - cy) / fy * near;

            targetCamera.projectionMatrix = PerspectiveOffCenter(left, right, bottom, top, near, far);

            Debug.Log($"[ICG] Applied intrinsics fx={fx:F2} fy={fy:F2} cx={cx:F2} cy={cy:F2} w={w} h={h} flipY={flipY}");
            _applied = true;
        }

        private static Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
        {
            float x = 2f * near / (right - left);
            float y = 2f * near / (top - bottom);
            float a = (right + left) / (right - left);
            float b = (top + bottom) / (top - bottom);
            float c = -(far + near) / (far - near);
            float d = -(2f * far * near) / (far - near);

            Matrix4x4 m = new Matrix4x4();
            m[0, 0] = x;  m[0, 2] = a;
            m[1, 1] = y;  m[1, 2] = b;
            m[2, 2] = c;  m[2, 3] = d;
            m[3, 2] = -1f;
            return m;
        }
    }
}
