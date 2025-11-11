using System.Collections;
using System.Collections.Generic;
using System;

using Unity.Collections.LowLevel.Unsafe;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

using coartec;
using coartec.MediaDisplay;
using UnityEngine.Serialization;

public class CameraImageUpdater : AbstractImageUpdater
{
    // Scene components and settings
    [Header("Scene")]
    //[Tooltip("Screen object in the scene")]
    //[SerializeField] protected GameObject screenObject;

    [SerializeField] [Tooltip("Camera object in the scene")]
    protected ARCameraManager cameraManager;
    [SerializeField] 
    private Text imageInfo;
    
    private Texture2D _cameraTexture;
    private RawImage _rawImage;
    private WebCamTexture _webcam;
    
    XRCpuImage.Transformation transformation = XRCpuImage.Transformation.MirrorY;

    private const TextureFormat ImFormat = UnityEngine.TextureFormat.RGBA32;

    // Called when the script instance is being loaded.
    private void Awake()
    {
        Debug.Log("[CameraScreenManager.cs] Awake()");
    }

    private void Start()
    {
    }
    
    // Update user choice for image source: webcam or image file
    private void Update()
    {
    }

    public override Texture2D GetUpdatedImage()
    {
        return CameraTexture;
    }
    
    public void OnClickUpdateScreen()
    {
        UpdateScreen();
    }
    
    unsafe void UpdateCameraImage()
    {
        // Attempt to get the latest camera image. If this method succeeds,
        // it acquires a native resource that must be disposed (see below).
        if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            Debug.Log("[CameraScreenManager.cs] Failed to acquire image");
            return;
        }

        // Display some information about the camera image
        imageInfo.text = string.Format(
            "Image info:\n\twidth: {0}\n\theight: {1}\n\tplaneCount: {2}\n\ttimestamp: {3}\n\tformat: {4}",
            image.width, image.height, image.planeCount, image.timestamp, image.format);

        // Once we have a valid XRCpuImage, we can access the individual image "planes"
        // (the separate channels in the image). XRCpuImage.GetPlane provides
        // low-overhead access to this data. This could then be passed to a
        // computer vision algorithm. Here, we will convert the camera image
        // to an RGBA texture and draw it on the screen.

        // Choose an RGBA format.
        // See XRCpuImage.FormatSupported for a complete list of supported formats.
        var format = ImFormat;

        if (_cameraTexture == null || _cameraTexture.width != image.width || _cameraTexture.height != image.height)
        {
            _cameraTexture = new Texture2D(image.width, image.height, format, false);
        }

        // Convert the image to format, flipping the image across the Y axis.
        // We can also get a sub rectangle, but we'll get the full image here.
        var conversionParams = new XRCpuImage.ConversionParams(image, format, transformation);

        // Texture2D allows us write directly to the raw texture data
        // This allows us to do the conversion in-place without making any copies.
        var rawTextureData = _cameraTexture.GetRawTextureData<byte>();
        try
        {
            image.Convert(conversionParams, new IntPtr(rawTextureData.GetUnsafePtr()), rawTextureData.Length);
        }
        finally
        {
            // We must dispose of the XRCpuImage after we're finished
            // with it to avoid leaking native resources.
            image.Dispose();
        }

        // Apply the updated texture data to our texture
        _cameraTexture.Apply();
    }
    
    // Updates the screen with the current texture
    private void UpdateScreen()
    {
        UpdateCameraImage();
        
        // Set the RawImage's texture so we can visualize it.
        _rawImage.texture = _cameraTexture;
    }

    public Texture2D CameraTexture
    {
        get
        {
            UpdateCameraImage(); 
            
            return _cameraTexture;
        }
    }
}
