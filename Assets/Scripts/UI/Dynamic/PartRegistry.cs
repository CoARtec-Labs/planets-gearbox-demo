using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoARtec.UI.Dynamic
{
    [Serializable]
    public class PartBinding
    {
        public string Key;       // 3d Object: "base", "ring", "sun", ...
        public GameObject Part;  // Reference to the GameObject in Assembly Scene
    }

    public class PartRegistry : MonoBehaviour
    {
        [Tooltip("Mapping from Part-Key (DB) to GameObject (Unity)")]
        public List<PartBinding> Parts = new List<PartBinding>();

        private Dictionary<string, GameObject> _map;

        private void Awake()
        {
            _map = new Dictionary<string, GameObject>(StringComparer.OrdinalIgnoreCase);
            foreach (var p in Parts)
            {
                if (!string.IsNullOrWhiteSpace(p.Key) && p.Part != null)
                    _map[p.Key] = p.Part;
            }
        }

        public GameObject GetPart(string key)
        {
            if (_map == null) return null;
            return _map.TryGetValue(key, out var go) ? go : null;
        }

        public IEnumerable<KeyValuePair<string, GameObject>> Enumerate()
        {
            if (_map == null) yield break;
            foreach (var kv in _map)
                yield return kv;
        }
    }
}
