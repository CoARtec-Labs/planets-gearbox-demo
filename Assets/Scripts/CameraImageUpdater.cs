using System.Collections;
using System.Collections.Generic;
using System;

using Unity.Collections.LowLevel.Unsafe;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

using Coartec.ImageUpdate;

/// <summary>
/// Implementation of an image updater receiving images from the AR camera object. 
/// </summary>
public class CameraImageUpdater : AbstractImageUpdater
{
    [SerializeField] [Tooltip("AR Camera object in the scene.")]
    protected ARCameraManager cameraManager;
    [SerializeField] [Tooltip("Text field to display some properties of the image.")]
    private Text imageInfo;
    
    private Texture2D _cameraTexture;

    private const XRCpuImage.Transformation TrafoMirrorY = XRCpuImage.Transformation.MirrorY;
    private const TextureFormat ImFormat = UnityEngine.TextureFormat.RGBA32;

    public override Texture2D GetUpdatedImage()
    {
        return CameraTexture;
    }
    
    private unsafe void UpdateCameraImage()
    {
        // Attempt to get the latest camera image. If this method succeeds,
        // it acquires a native resource that must be disposed (see below).
        if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            Debug.Log("[CameraImageUpdater.cs] Failed to acquire image");
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
        var conversionParams = new XRCpuImage.ConversionParams(image, format, TrafoMirrorY);

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

    private Texture2D CameraTexture
    {
        get
        {
            UpdateCameraImage(); 
            return _cameraTexture;
        }
    }
}
