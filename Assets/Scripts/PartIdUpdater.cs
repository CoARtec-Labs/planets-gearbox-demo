using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Serialization;

using Coartec.Detection;

using UI.Views;

/// <summary>
/// This script is responsible for updating the detector filter with current part ID.
/// </summary>
public class PartIdUpdater : MonoBehaviour
{
    [SerializeField] private DetectionManager detector;
    // [SerializeField] private UIRootBase uiRoot;
    
    // Start is called before the first frame update
    void Start()
    {
        detector.ClassIdx = -1; // default = do not filter
    }

    // Update is called once per frame
    void Update()
    {
        detector.ClassIdx = UIRootBase.SearchObjectClassId;
    }
}