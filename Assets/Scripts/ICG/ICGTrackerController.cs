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

        private bool _started;

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

            if (targetTransform == null)
                targetTransform = this.transform; // fallback

        }
        void Update()
        {
            if (!_started) return;

            if (ICGNative.TryGetPose(bodyName, out var t, out var q, out var ts))
            {
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
        }

        void OnDestroy()
        {
            if (_started)
            {
                ICGNative.Stop();
                _started = false;
            }
            ICGNative.Destroy();
        }
    }
}
