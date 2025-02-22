using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Barracuda;
using UnityEngine.UI;
using UnityEditor;

using CJM.DeepLearningImageProcessor;

using BBox2DToolkit;
using UnityEngine.Serialization;

public class ScriptDetector : MonoBehaviour
{

    public NNModel modelAsset;
    public int batch_size = 9999;
    public int n_channels = 9999;
    public int width = 9999;
    public int height = 9999;

    public Texture2D testTexture2D;
    public RenderTexture testRenderTexture;

    private float[] predictedValues;

    public float[] columnZero;

    private int numProposals = -1;

    public List<float> filteredScores;
    public List<float> filteredClasses;
    private List<float[]> filteredPredictions;
    private List<BBox2D> filteredBoxes, boxesOut;
    private BBox2DInfo[] bboxInfoArray;
    private OnnxInterface _onnxInterface;
    public BoundingBox2DVisualizer boundingBoxVisualizer;
    public ImageProcessor imageProcessor;
    public MeshRenderer screenRenderer;
    public RawImage rawImage;

    private int targetDim;
    private Vector2Int offset;

    public int updateRateMultiplier = 60;
    public float confidenceThreshold = 0.5F;
    public float nmsThreshold = 0.5F;
    
    private int timer = 0;
    
    [SerializeField, Tooltip("JSON file with bounding box colormaps")]
    private TextAsset colormapFile;
    
    // Serializable classes to store color map information from JSON
    [System.Serializable]
    class Colormap
    {
        public string label;
        public List<float> color;
    }

    [System.Serializable]
    class ColormapList
    {
        public List<Colormap> items;
    }

    // List to store label and color pairs for each class
    private List<(string, Color)> colormapList = new List<(string, Color)>();


    void Start()
    {   
        if (width == height)
        {
            targetDim = width;
        } else
        {
            throw new System.Exception("Given image dimensions must be square");
        }

        _onnxInterface = new OnnxInterface(modelAsset, width, height, n_channels, batch_size);

        LoadColorMapList(); 
    }

    void Update()
    {

    }

    public void BBoxArrayTest()
    {

        bboxInfoArray = new BBox2DInfo[2];

        bboxInfoArray[0] = new BBox2DInfo(new BBox2D(0, 0, 40, 20, 1, 0.9F), "box1");
        bboxInfoArray[1] = new BBox2DInfo(new BBox2D(100, 100, 20, 40, 2, 0.9F), "box2");

        Vector2Int imageDims = new Vector2Int(width, height);
        
        UpdateBoundingBoxes(imageDims);
        
        boundingBoxVisualizer.UpdateBoundingBoxVisualizations(bboxInfoArray);

    }

    public void RunPrediction_bak()
    {
        Texture2D imageTexture = (Texture2D)screenRenderer.material.mainTexture;

        Vector2Int inputDims = new Vector2Int(targetDim, targetDim);
        Vector2Int imageDims = new Vector2Int(imageTexture.width, imageTexture.height);

        offset = (imageDims - inputDims) / 2;

        // Convert from Texture2D to RenderTexture
        RenderTexture imageTextureRender = new RenderTexture(imageTexture.width, imageTexture.height, 0, RenderTextureFormat.ARGBHalf);
        RenderTexture.active = imageTextureRender;
        Graphics.Blit(imageTexture, imageTextureRender);
        RenderTexture.active = null;

        // Texture2D inputTexture = new Texture2D(inputDims.x, inputDims.y, TextureFormat.DXT1, true);
        RenderTexture inputTextureRender = new RenderTexture(imageTexture.width, imageTexture.height, 0, RenderTextureFormat.ARGBHalf);

        // Prepare and process the input texture
        ProcessImageShader(imageTextureRender, inputTextureRender, imageDims, inputDims);

        // Convert from RenderTexture to Texture2D
        Texture2D texture2D = new Texture2D(inputTextureRender.width, inputTextureRender.height, TextureFormat.ARGB32, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = inputTextureRender;
        texture2D.ReadPixels(new Rect(0, 0, inputTextureRender.width, inputTextureRender.height), 0, 0);
        texture2D.Apply();
        RenderTexture.active = currentRT; // Restore the previously active RenderTexture

        texture2D.name = "texture debugger";

        rawImage.texture = inputTextureRender;

        UpdateBoundingBoxes(imageDims);
        boundingBoxVisualizer.UpdateBoundingBoxVisualizations(bboxInfoArray);

    }

    public void RunPrediction_online()
    {
        
        Debug.Log("[ObjectDetector] Call to RunPrediction_online ...");

        Texture imageTexture = screenRenderer.material.mainTexture;
        
        if (!screenRenderer.material.mainTexture)
        {
            Debug.Log("Texture of Screen Renderer is not set. Returning.");
            return;
        }
        
        /*
        else if (System.Object.ReferenceEquals(screenRenderer.material.mainTexture.GetType(), typeof(Texture2D)))
        {
            imageTexture = (Texture2D)screenRenderer.material.mainTexture;
        }
        else if (System.Object.ReferenceEquals(screenRenderer.material.mainTexture.GetType(), typeof(WebCamTexture)))
        {
            imageTexture = WebCamTextureToTexture2d((WebCamTexture)screenRenderer.material.mainTexture);
        }
        else
        {
            throw new System.Exception("Texture of Screen Renderer is invalid.");
        }
        */

        RenderTexture imageTextureRender = new RenderTexture(imageTexture.width, imageTexture.height, 0);
        RenderTexture.active = imageTextureRender;
        Graphics.Blit(imageTexture, imageTextureRender);
        RenderTexture.active = null;

        Vector2Int imageDims = new Vector2Int(imageTexture.width, imageTexture.height);
        // var inputDims = imageProcessor.CalculateInputDims(imageDims, targetDim);
        Vector2Int inputDims = new Vector2Int(targetDim, targetDim);

        // RenderTexture inputTexture = RenderTexture.GetTemporary(inputDims.x, inputDims.y, 0, RenderTextureFormat.ARGBHalf);
        RenderTexture inputTextureRender = new RenderTexture(inputDims.x, inputDims.y, 0, RenderTextureFormat.ARGBHalf);
        
        // Prepare and process the input texture
        ProcessImageShader(imageTextureRender, inputTextureRender, imageDims, inputDims);

        // Convert from RenderTexture to Texture2D
        Texture2D inputTexture = new Texture2D(inputDims.x, inputDims.y, TextureFormat.ARGB32, false);
        // RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = inputTextureRender;
        inputTexture.ReadPixels(new Rect(0, 0, inputDims.x, inputDims.y), 0, 0);
        inputTexture.Apply();
        // RenderTexture.active = currentRT; // Restore the previously active RenderTexture
        RenderTexture.active = null;

        rawImage.texture = inputTexture;

        Tensor output = _onnxInterface.RunPrediction(inputTexture);

        // numProposals = output.channels;
        predictedValues = output.AsFloats();

        // columnZero = GetColumn(0);

        filteredBoxes = FilterClasses(confidenceThreshold);

        List<int> proposal_indices = BBox2DUtility.NMSSortedBoxes(filteredBoxes, nmsThreshold);
        
        bboxInfoArray = proposal_indices
                .Select(index => filteredBoxes[index])
                .Select(bbox => new BBox2DInfo(
                    new BBox2D(bbox.x0, 320-bbox.y0, bbox.width, bbox.height, bbox.index, bbox.prob),
                    $"class {bbox.index}"))
                .ToArray();

        print($"Bboxes out: {bboxInfoArray.Length}");

        UpdateBoundingBoxes(imageDims);

        boundingBoxVisualizer.UpdateBoundingBoxVisualizations(bboxInfoArray);

        output.Dispose();
    }

    public void RunPrediction_trigger()
    {
        Debug.Log("[ObjectDetector] Call to RunPrediction_trigger ...");

        Texture imageTexture = screenRenderer.material.mainTexture;
        
        if (!screenRenderer.material.mainTexture)
        {
            Debug.Log("Texture of Screen Renderer is not set. Returning.");
            return;
        }

        RenderTexture imageTextureRender = new RenderTexture(imageTexture.width, imageTexture.height, 0);
        RenderTexture.active = imageTextureRender;
        Graphics.Blit(imageTexture, imageTextureRender);
        RenderTexture.active = null;

        Vector2Int imageDims = new Vector2Int(imageTexture.width, imageTexture.height);
        // var inputDims = imageProcessor.CalculateInputDims(imageDims, targetDim);
        Vector2Int inputDims = new Vector2Int(targetDim, targetDim);
        
        offset = (imageDims - inputDims) / 2;
        
        // RenderTexture inputTexture = RenderTexture.GetTemporary(inputDims.x, inputDims.y, 0, RenderTextureFormat.ARGBHalf);
        RenderTexture inputTextureRender = new RenderTexture(inputDims.x, inputDims.y, 0, RenderTextureFormat.ARGBHalf);

        // Texture2D inputTexture = new Texture2D(inputDims.x, inputDims.y, TextureFormat.DXT1, true);

        // Prepare and process the input texture
        ProcessImageShader(imageTextureRender, inputTextureRender, imageDims, inputDims);

        // Convert from RenderTexture to Texture2D
        Texture2D inputTexture = new Texture2D(inputDims.x, inputDims.y, TextureFormat.ARGB32, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = inputTextureRender;
        inputTexture.ReadPixels(new Rect(0, 0, inputDims.x, inputDims.y), 0, 0);
        inputTexture.Apply();
        RenderTexture.active = currentRT; // Restore the previously active RenderTexture

        rawImage.texture = inputTexture;

        Tensor output = _onnxInterface.RunPrediction(inputTexture);

        numProposals = output.channels;
        predictedValues = output.AsFloats();

        columnZero = GetColumn(0);
        
        print($"array shape, rows: {predictedValues.GetLength(0)}");

        // output.Print("Output Tensor");
        //
        // print("Stats Col 0");
        // PrintArrayStats(GetColumn(0));
        //
        // print("Stats Col 4");
        // PrintArrayStats(GetColumn(4));
        //
        // print("Stats Col 8");
        // PrintArrayStats(GetColumn(8));

        filteredBoxes = FilterClasses(confidenceThreshold);

        filteredScores = filteredPredictions.Select(x => x[5]).ToList();
        filteredClasses = filteredPredictions.Select(x => x[4]).ToList();

        List<int> proposal_indices = BBox2DUtility.NMSSortedBoxes(filteredBoxes, nmsThreshold);
        
        bboxInfoArray = proposal_indices
                .Select(index => filteredBoxes[index])
                .Select(bbox => new BBox2DInfo(
                    new BBox2D(bbox.x0, 320-bbox.y0, bbox.width, bbox.height, bbox.index, bbox.prob),
                    colormapList[bbox.index].Item1, colormapList[bbox.index].Item2))
                .ToArray();

        print($"Bboxes out: {bboxInfoArray.Length}");

        // Swap x, y since display is in portrait
        offset = new Vector2Int(offset.y, offset.x);
        // UpdateBoundingBoxes(new Vector2Int(inputDims.y, inputDims.x)); 

        boundingBoxVisualizer.UpdateBoundingBoxVisualizations(bboxInfoArray);
        
        output.Dispose();
    }


    private List<BBox2D> FilterClasses(float objectScoreThrs)
    {

        int filtIdx = 0;

        List<BBox2D> bBoxes = new List<BBox2D>();
        List<float[]> predictions = new();

        List<float> bboxCoord = new();
        List<float> classScores = new();
        List<float> overallScores;

        float objectScore, bestScore, bestClass;

        for (int i = 0; i < numProposals; i++)
        {
            bboxCoord.Clear();
            classScores.Clear();

            bboxCoord.Add(predictedValues[i + numProposals * 0]);
            bboxCoord.Add(predictedValues[i + numProposals * 1]);
            bboxCoord.Add(predictedValues[i + numProposals * 2]);
            bboxCoord.Add(predictedValues[i + numProposals * 3]);
            
            objectScore = predictedValues[i + numProposals * 4];

            classScores.Add(predictedValues[i + numProposals * 5]);
            classScores.Add(predictedValues[i + numProposals * 6]);
            classScores.Add(predictedValues[i + numProposals * 7]);
            classScores.Add(predictedValues[i + numProposals * 8]);

            if (objectScore < objectScoreThrs)
            {
                continue;
            }
            
            overallScores = classScores.Select(x => x * objectScore).ToList();

            bestScore = overallScores.Max();
            bestClass = overallScores.IndexOf(bestScore);

            // width and height
            float w = bboxCoord[2];
            float h = bboxCoord[3];
            
            // top left position
            float x = bboxCoord[0] - 0.5f * w;
            float y = bboxCoord[1] - 0.5f * h;

            // BBox2D box = new BBox2D(x, y, w, h, 0, 0);
            float[] pred = new float[6];

            pred[0] = x;
            pred[1] = y;
            pred[2] = w;
            pred[3] = h;
            pred[4] = bestClass;
            pred[5] = bestScore;

            predictions.Add(pred);

            bBoxes.Add(new BBox2D(x, y, w, h, (int)bestClass, bestScore));

            filtIdx ++;

        }

        // Sort predictions by score 
        filteredPredictions = predictions.OrderBy(x => x[5]).ToList();

        return bBoxes.OrderBy(x => x.prob).ToList();
    }


    private void PrintArrayStats(float[] array)
    {
        // Compute min, max, and mean
        float min = array.Min();
        float max = array.Max();
        float mean = array.Average();

        // Display the results
        print($"min: {min}, max: {max}, mean: {mean}");
    }

    private float[] GetColumn(int colIdx)
    {
        float[] arrayCol = new float[numProposals];

        int predIdx;

        for (int i = 0; i < numProposals; i++)
        {
            predIdx = colIdx * numProposals + i;

            arrayCol[i] = predictedValues[predIdx];
        }

        return arrayCol;
    }


    private void ProcessImageShader(RenderTexture sourceImage, RenderTexture targetImage, Vector2Int sourceDims, Vector2Int targetDims)
    {

        // Calculate the offset for cropping the input image
        Vector2 offset = (sourceDims - targetDims) / 2;

        // Create a temporary render texture to store the cropped image
        // RenderTexture sourceImageRenTex = RenderTexture.GetTemporary(sourceDims.x, sourceDims.y, 0, RenderTextureFormat.ARGBHalf);
        // Graphics.Blit(sourceImage, sourceImageRenTex);

        // Calculate the scaled offset and size for cropping the input image
        Vector2 scaledOffset = offset / (Vector2)sourceDims;
        Vector2 scaledSize = targetDims / (Vector2)sourceDims;

        // Create offset and size arrays for the Shader
        float[] offsetArray = new float[] { scaledOffset.x, scaledOffset.y };
        float[] sizeArray = new float[] { scaledSize.x, scaledSize.y };

        // Crop and normalize the input image using Shaders
        imageProcessor.CropImageShader(sourceImage, targetImage, offsetArray, sizeArray);

        // targetImage = sourceImage;
        //imageProcessor.ProcessImageShader(targetImage);
    }

    /// <summary>
    /// Update the bounding boxes based on the input dimensions and screen dimensions.
    /// </summary>
    /// <param name="inputDims">The input dimensions for processing</param>
    private void UpdateBoundingBoxes(Vector2Int inputDims)
    {
        // Check if the screen is mirrored
        bool mirrorScreen = screenRenderer.transform.localScale.z == -1;

        // Get the screen dimensions
        Vector2 screenDims = new Vector2(screenRenderer.transform.localScale.x, screenRenderer.transform.localScale.y);

        // Scale and position the bounding boxes based on the input and screen dimensions
        for (int i = 0; i < bboxInfoArray.Length; i++)
        {
            bboxInfoArray[i].bbox = RotateBBoxPlus90Deg(bboxInfoArray[i].bbox);
            bboxInfoArray[i].bbox = BBox2DUtility.ScaleBoundingBox(bboxInfoArray[i].bbox, inputDims, screenDims, offset, mirrorScreen);
        }
    }

    private BBox2D RotateBBoxPlus90Deg(BBox2D bboxIn)
    {
        BBox2D bboxOut = new BBox2D();

        bboxOut.x0 = targetDim - bboxIn.y0; // target dim is the square size of the cropped detection image
        bboxOut.y0 = bboxIn.width + bboxIn.x0;
        bboxOut.width = bboxIn.height;
        bboxOut.height = bboxIn.width;

        return bboxOut;
    }

    public Texture2D WebCamTextureToTexture2d(WebCamTexture webCamTexture)
    {
        Texture2D texture2D = new Texture2D(webCamTexture.width, webCamTexture.height);
        texture2D.SetPixels32(webCamTexture.GetPixels32());

        return texture2D;
    }
    
    
    /// <summary>
    /// Load the color map list from the JSON file
    /// <summary>
    private void LoadColorMapList()
    {
        if (IsColorMapListJsonNullOrEmpty())
        {
            Debug.LogError("Class labels JSON is null or empty.");
            return;
        }

        ColormapList colormapObj = DeserializeColorMapList(colormapFile.text);
        UpdateColorMap(colormapObj);
    }

    /// <summary>
    /// Check if the color map JSON file is null or empty
    /// <summary>
    private bool IsColorMapListJsonNullOrEmpty()
    {
        return colormapFile == null || string.IsNullOrWhiteSpace(colormapFile.text);
    }

    /// <summary>
    /// Deserialize the color map list from the JSON string
    /// <summary>
    private ColormapList DeserializeColorMapList(string json)
    {
        try
        {
            return JsonUtility.FromJson<ColormapList>(json);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to deserialize class labels JSON: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Update the color map list with deserialized data
    /// <summary>
    private void UpdateColorMap(ColormapList colormapObj)
    {
        if (colormapObj == null)
        {
            return;
        }

        // Add label and color pairs to the colormap list
        foreach (Colormap colormap in colormapObj.items)
        {
            Color color = new Color(colormap.color[0], colormap.color[1], colormap.color[2]);
            colormapList.Add((colormap.label, color));
        }
    }

}
