using System;
using coartec;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Image Updater passing on the image currently on the screen.
/// </summary>
public class WebcamImageUpdater : AbstractImageUpdater
{
    [SerializeField]
    private MeshRenderer imageScreen;

    private Texture2D _imageTexture;

    private void Awake()
    {
        _imageTexture = new Texture2D(0, 0, TextureFormat.ARGB32, false);
    }
    
    public override Texture2D GetUpdatedImage()
    {
        var webcam = imageScreen.material.mainTexture as WebCamTexture;
        
        if (webcam  is null)
            throw new System.Exception("ImageScreen.material.mainTexture is not WebCamTexture");
        
        _imageTexture.Reinitialize(webcam.width, webcam.height);
        _imageTexture.SetPixels(webcam.GetPixels());
        _imageTexture.Apply();

        return _imageTexture;
    }
}
