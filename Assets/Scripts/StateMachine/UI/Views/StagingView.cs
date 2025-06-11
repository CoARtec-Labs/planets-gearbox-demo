using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary>
/// The StagingView class.
/// This classe is used to (de)activate the staging UI.
/// </summary>
public class StagingView : BaseViewSingleton<StagingView>
{
    // Events to attach to.
    public static UnityAction OnAssemblyClicked;

    public TMP_Text searchTitleBanner;
    
    public static int SearchObjectClassId = -1;
    public int searchObjectClassId = -1;
    
    [Header("References to files")]
    [SerializeField, Tooltip("JSON file with bounding box color maps")]
    private TextAsset jsonColormapFile;

    private List<(string, Color)> _colormapList = new();
    
    // Serializable class to store color map information
    [System.Serializable]
    class Colormap
    {
        public string label;
        public List<float> color;
    }

    // Serializable class to store multiple color maps.
    [System.Serializable]
    class ColormapList
    {
        public List<Colormap> items;
    }
    
    private new void Awake()
    {
        base.Awake();
        
        LoadColorMapList();
    }

    private void OnEnable()
    {
        // Transform button = this.transform.root.gameObject.transform.Find("PartsDetection/Canvas/SearchTitle");
        
        // Transform title = transform.Find("AssemblyMenu/Canvas/Title");
        // Transform description = transform.Find("AssemblyMenu/Canvas/Description");

        // Transform title = menu.gameObject.GetChildGameObjects("Title");

        string name;
        
        if (SearchObjectClassId == -1)
        {
            name = "allgemein";
        }
        else
        {
            name = _colormapList[SearchObjectClassId].Item1;
        }
        
        searchTitleBanner.GetComponent<TMP_Text>().SetText($"Suche: {name}");
    }

    private void Update()
    {
        //SearchObjectClassId = searchObjectClassId;
    }
    
    /// <summary>
    /// Method for assembly button.
    /// </summary>
    public void AssemblyClick()
    {
        OnAssemblyClicked?.Invoke();
    }
    
    // Load the color map list from the JSON file
    private void LoadColorMapList()
    {
        if (IsColorMapListJsonNullOrEmpty())
        {
            Debug.LogError("Class labels JSON is null or empty.");
            return;
        }

        ColormapList colormapObj = DeserializeColorMapList(jsonColormapFile.text);
        UpdateColorMap(colormapObj);
    }
    
    // Check if the color map JSON file is null or empty
    private bool IsColorMapListJsonNullOrEmpty()
    {
        return jsonColormapFile == null || string.IsNullOrWhiteSpace(jsonColormapFile.text);
    }

    // Deserialize the color map list from the JSON string
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
        
    // Update the color map list with deserialized data
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
            _colormapList.Add((colormap.label, color));
        }
    }

    
}
