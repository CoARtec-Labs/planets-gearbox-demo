using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;
using UnityEngine.UI;

using coartec.MediaDisplay;

public class DisplayScreenManager : BaseScreenManager
{
    // [Header("GUI")]
    // [Tooltip("Toggle to use webcam as input source")]
    // [SerializeField] private Toggle useWebcamToggle;
    // [Tooltip("Dropdown menu with available webcam devices")]
    // [SerializeField] private Dropdown webcamDropdown;

    private void OnEnable()
    {
        base.OnEnable();
        UpdateDisplay();
    }
    
    // Called when the script instance is being loaded.
    private void Awake()
    {
        Debug.Log("[DisplayScreenManager.cs] Awake()");

        Initialize();
        UpdateDisplay();
        // InitializeDropdown();
    }

    // Update the useWebcamToggle to match the useWebcam value
    private void UpdateUseWebcamToggle()
    {
        // // Check if the useWebcamToggle value matches the useWebcam value before updating
        // if (useWebcamToggle.isOn != useWebcam)
        // {
        //     // Set the useWebcamToggle value without invoking its event
        //     useWebcamToggle.SetIsOnWithoutNotify(useWebcam);
        // }
    }
    
    // Handle the texture change event.
    protected override void HandleMainTextureChanged(Material material)
    {
        // Call the base class implementation first
        base.HandleMainTextureChanged(material);
        
        // Update the GUI webcam toggle
        UpdateUseWebcamToggle();
    }
}
