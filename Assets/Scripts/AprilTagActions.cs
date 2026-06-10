using System.Collections.Generic;
using UnityEngine;

namespace Instructions
{
    /// <summary>
    /// AprilTag counterpart of ARTrackedImagesActions.
    /// Reads detection results from a WebcamAprilTag component each frame and
    /// spawns / moves / (optionally) despawns the parts assembly accordingly.
    /// No ARFoundation required — runs on desktop with a regular webcam.
    /// </summary>
    public class AprilTagActions : MonoBehaviour
    {
        [Header("Detector")]
        [Tooltip("WebcamAprilTag component from the coartec.apriltag-interface package.")]
        [SerializeField] private WebcamAprilTag tagDetector;

        [Header("Spawning")]
        [Tooltip("Prefab to spawn when a tag is first detected (same prefab used by QR flow).")]
        [SerializeField] private GameObject partsAssembly;

        [Tooltip("Multiplier applied to tag (x,y,z) positions reported by the detector (in meters).")]
        [SerializeField] private float positionScale = 1f;

        [Tooltip("If true, destroys the spawned assembly when the tag has not been seen for `loseTrackTimeout` seconds.")]
        [SerializeField] private bool despawnOnTagLost = false;

        [Tooltip("Seconds with no detection before despawning (only if despawnOnTagLost is true).")]
        [SerializeField] private float loseTrackTimeout = 1.0f;

        // tagId -> spawned GameObject
        private readonly Dictionary<int, GameObject> spawnedTags = new Dictionary<int, GameObject>();

        // tagId -> last time it was seen (for despawn timing)
        private readonly Dictionary<int, float> lastSeenTime = new Dictionary<int, float>();

        private void Start()
        {
            if (tagDetector == null)
                Debug.LogError("[AprilTagActions] tagDetector reference is not set in the Inspector!");
            if (partsAssembly == null)
                Debug.LogError("[AprilTagActions] partsAssembly prefab is not set in the Inspector!");
        }

        private void Update()
        {
            if (tagDetector == null || partsAssembly == null) return;

            var results = tagDetector.LiveResults;
            if (results == null) return;

            // ===== ADDED / UPDATED =====
            foreach (var tag in results)
            {
                if (tag.valid != 1) continue;

                Vector3 pos = new Vector3(
                    (float)tag.positionX * positionScale,
                    (float)tag.positionY * positionScale,
                    (float)tag.positionZ * positionScale
                );

                Quaternion rot = new Quaternion(
                    (float)tag.quaternionX,
                    (float)tag.quaternionY,
                    (float)tag.quaternionZ,
                    (float)tag.quaternionW
                );

                lastSeenTime[tag.id] = Time.time;

                if (!spawnedTags.ContainsKey(tag.id) || spawnedTags[tag.id] == null)
                {
                    // ADDED
                    Debug.Log($"[AprilTagActions] New tag {tag.id} at {pos}");
                    GameObject go = Instantiate(partsAssembly, pos, rot);
                    spawnedTags[tag.id] = go;
                }
                else
                {
                    // UPDATED
                    GameObject go = spawnedTags[tag.id];
                    go.transform.position = pos;
                    go.transform.rotation = rot;
                }
            }

            // ===== REMOVED (optional, time-based) =====
            if (despawnOnTagLost)
            {
                var toRemove = new List<int>();
                foreach (var kvp in spawnedTags)
                {
                    int id = kvp.Key;
                    if (!lastSeenTime.ContainsKey(id)) continue;

                    if (Time.time - lastSeenTime[id] > loseTrackTimeout)
                    {
                        if (kvp.Value != null) Destroy(kvp.Value);
                        toRemove.Add(id);
                        Debug.Log($"[AprilTagActions] Tag {id} lost — despawned.");
                    }
                }
                foreach (int id in toRemove)
                {
                    spawnedTags.Remove(id);
                    lastSeenTime.Remove(id);
                }
            }
        }

        private void OnDisable()
        {
            foreach (var kvp in spawnedTags)
            {
                if (kvp.Value != null) Destroy(kvp.Value);
            }
            spawnedTags.Clear();
            lastSeenTime.Clear();
        }
    }
}