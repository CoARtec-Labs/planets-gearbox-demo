using UnityEngine;

namespace ICG
{
    public class ICGTrackerController : MonoBehaviour
    {
        [Header("Paths (absolute for PoC)")]
        public string metaDirectory;
        public string sequenceDirectory;

        [Header("Tracking")]
        public string bodyName = "lid";  // anpassen an dein Body-Name
        public bool startOnPlay = true;

        [Header("Auto-Scan (Native)")]
        [Tooltip("Max. CPU-Budget pro Frame in der SCANNING-Phase (ms).")]
        public int scanBudgetMs = 6;
        [Tooltip("Raster-Schrittweite im Depth-Bild (Pixel). Größer = schneller, aber weniger robust.")]
        public int scanGridStepPx = 40;
        [Tooltip("Anzahl Yaw-Rotationen (0..360°) pro Rasterpunkt.")]
        public int scanYawSteps = 18;

        [Header("Overlay Visibility")]
        [Tooltip("Wenn keine Pose verfügbar (IDLE/SCANNING), wird das Overlay ausgeblendet.")]
        public bool hideOverlayWhenNoPose = true;
        [Tooltip("Optional: Renderers, die zum Overlay gehören. Wenn leer, werden sie automatisch unter targetTransform gesucht.")]
        public Renderer[] overlayRenderers;

        [Header("Debug")]
        public bool logStateChanges = false;

        private bool _started;
        private ICGNative.TrackingState _lastState = (ICGNative.TrackingState)(-1);

        [Header("Apply Pose")]
        public Transform targetTransform;
        public Vector3 positionScale = Vector3.one;   // falls Units nicht passen
        public Vector3 positionOffset = Vector3.zero; // falls du offset brauchst
        public bool applyRotation = true;
        public bool applyPosition = true;
        public enum AxisMapping { Direct, OpenCV_to_Unity }
        public AxisMapping axisMapping = AxisMapping.OpenCV_to_Unity;

        [Header("Reference Frame")]
        [Tooltip("Wenn gesetzt: Pose wird relativ zu diesem Transform angewendet (typisch: OverlayCamera). " +
                 "Damit ist die Szene robuster, auch wenn die Kamera nicht bei (0,0,0) steht.")]
        public Transform referenceFrame;


        void Start()
        {
            if (!startOnPlay) return;

            Debug.Log("[ICG] SetSequenceDirectory...");
            ICGNative.SetSequenceDirectory(sequenceDirectory);

            Debug.Log("[ICG] Create...");
            int okCreate = ICGNative.Create(metaDirectory);
            if (okCreate == 0)
            {
                Debug.LogError("[ICG] Create failed: " + ICGNative.GetLastError());
                return;
            }

            Debug.Log("[ICG] AddBody...");
            int okAdd = ICGNative.AddBody(bodyName);
            if (okAdd == 0)
            {
                Debug.LogError("[ICG] AddBody failed: " + ICGNative.GetLastError());
                return;
            }

            Debug.Log("[ICG] Start...");
            int okStart = ICGNative.Start();
            if (okStart == 0)
            {
                Debug.LogError("[ICG] Start failed: " + ICGNative.GetLastError());
                return;
            }

            _started = true;
            Debug.Log("[ICG] Started OK.");

            // Apply scan tuning (optional)
            if (scanBudgetMs > 0) ICGNative.SetScanBudgetMs(scanBudgetMs);
            if (scanGridStepPx > 0) ICGNative.SetScanGridStepPx(scanGridStepPx);
            if (scanYawSteps > 0) ICGNative.SetScanYawSteps(scanYawSteps);

            // Start auto-initialization (SCANNING). Pose stays invalid until native finds a good pose.
            int okTrack = ICGNative.StartTracking();
            if (okTrack == 0)
            {
                Debug.LogError("[ICG] StartTracking failed: " + ICGNative.GetLastError());
            }

            if (targetTransform == null)
                targetTransform = this.transform; // fallback

            // Setup overlay renderers
            if (overlayRenderers == null || overlayRenderers.Length == 0)
            {
                overlayRenderers = targetTransform.GetComponentsInChildren<Renderer>(true);
            }

            // Start hidden until we have a valid pose
            if (hideOverlayWhenNoPose)
                SetOverlayVisible(false);

        }
        void Update()
        {
            if (!_started) return;

            var state = ICGNative.GetTrackingState();
            if (state != _lastState)
            {
                _lastState = state;
                if (logStateChanges)
                    Debug.Log($"[ICG] State = {state}");
            }

            if (ICGNative.TryGetPose(bodyName, out var t, out var q, out var ts))
            {
                if (hideOverlayWhenNoPose)
                    SetOverlayVisible(true);

                if (Time.frameCount % 30 == 0)
                {
                    Debug.Log($"[ICG] pose t=({t[0]:F3},{t[1]:F3},{t[2]:F3}) q=({q[0]:F3},{q[1]:F3},{q[2]:F3},{q[3]:F3}) ts={ts:F3}");
                }
                Vector3 pos;
                Quaternion rot;

                if (axisMapping == AxisMapping.Direct)
                {
                    pos = new Vector3(t[0], t[1], t[2]);
                    rot = new Quaternion(q[0], q[1], q[2], q[3]);
                }
                else
                {
                    // OpenCV camera coords (x right, y down, z forward)
                    // Unity coords (x right, y up, z forward) but left-handed
                    // Basis-Konvertierung: Spiegelung in der XZ-Ebene (y -> -y)
                    pos = new Vector3(t[0], -t[1], t[2]);

                    // Rotation passend zur Spiegelung S = diag(1,-1,1):
                    // q' = (-qx, qy, -qz, qw)
                    rot = new Quaternion(-q[0], q[1], -q[2], q[3]);
                }

                pos = Vector3.Scale(pos, positionScale) + positionOffset;

                if (referenceFrame != null)
                {
                    if (applyPosition) targetTransform.position = referenceFrame.TransformPoint(pos);
                    if (applyRotation) targetTransform.rotation = referenceFrame.rotation * rot;
                }
                else
                {
                    if (applyPosition) targetTransform.localPosition = pos;
                    if (applyRotation) targetTransform.localRotation = rot;
                }
            }
            else
            {
                if (hideOverlayWhenNoPose)
                    SetOverlayVisible(false);
            }
        }

        private void SetOverlayVisible(bool visible)
        {
            if (overlayRenderers == null) return;
            for (int i = 0; i < overlayRenderers.Length; i++)
            {
                var r = overlayRenderers[i];
                if (r == null) continue;
                r.enabled = visible;
            }
        }

        void OnDestroy()
        {
            if (_started)
            {
                // Optional: stop tracking state machine first
                ICGNative.StopTracking();
                ICGNative.Stop();
                _started = false;
            }
            ICGNative.Destroy();
        }
    }
}
