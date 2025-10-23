using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Serialization;

using coartec.detection;

/// <summary>
/// This script is responsible for updating the detector with current part ID.
/// </summary>
public class PartsUpdater : MonoBehaviour
{
    [SerializeField] private StagingView stagingView;
    [SerializeField] private DetectionManager detector;
    
    // Start is called before the first frame update
    void Start()
    {
        detector.ClassIdx = -1;
    }

    // Update is called once per frame
    void Update()
    {
        detector.ClassIdx = StagingView.SearchObjectClassId; // from static class
    }
}