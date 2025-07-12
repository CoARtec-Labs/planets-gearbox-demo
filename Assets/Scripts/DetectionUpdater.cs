using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Serialization;

namespace coartec.detection
{
    public class DetectionUpdater : MonoBehaviour
    {
        [SerializeField] private StagingView stagingView;
        
        public int updateRateMultiplier = 0;

        public DetectionManager detector;

        private int _timer;

        // Start is called before the first frame update
        void Start()
        {
            _timer = 0;
        }

        // Update is called once per frame
        void Update()
        {
            _timer++;

            if (_timer % updateRateMultiplier == 0)
            {
                detector.ClassIdx = StagingView.SearchObjectClassId; // from static class
                detector.LaunchDetection();

                _timer = 0;
            }
        }
    }
}