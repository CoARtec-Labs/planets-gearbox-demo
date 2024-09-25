using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ImageLoader : MonoBehaviour
{

    public UnityEngine.UI.Image displayImage;

    public Texture2D texPreset;


    // Start is called before the first frame update
    void Start()
    {
        displayImage.sprite = LoadImage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Sprite LoadImage()
    {
        /*
        string filename = "Assets/Images/pen/pen_01.jpg";
        var rawData = System.IO.File.ReadAllBytes(filename);

        // Create an empty Texture; size doesn't matter
        Texture2D tex = new Texture2D(2, 2); 
        tex.LoadImage(rawData);
        */

        Sprite sprite = Sprite.Create(texPreset, new Rect(0, 0, texPreset.width, texPreset.height), 
            new Vector2(0.5f, 0.5f));

        return sprite;
    }

}
