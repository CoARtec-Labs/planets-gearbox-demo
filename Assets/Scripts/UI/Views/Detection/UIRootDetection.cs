using System.Collections.Generic;
using UI.Data;
using UnityEngine;

namespace UI.Views.Detection
{
    /// <summary>
    /// UI Root class, used for storing references to UI views and managing activation of scene objects.
    /// </summary>
    public class UIRootDetection : UIRootSingleton<UIRootDetection>
    {
        [SerializeField]
        private DetectionView detectionView;
        public DetectionView DetectionView => detectionView;
    
        public List<GameObject> sceneObjects = new List<GameObject>();
        public bool activateScene = false;

        // public static int SearchObjectClassId = -1;
    
        private void Update()
        {
            // Trigger scene objects activation
            if (activateScene)
            {
                ActivateSceneObjects();
                DetectionView.Instance.ShowView();
                activateScene = false;
            }
        }
    
        public void ActivateSceneObjects()
        {
            Debug.Log("[UIRootDetection.cs] Activating relevant scene objects.");
        
            sceneObjects.ForEach(item => item.gameObject.SetActive(true));
        }
    
        public void DeactivateSceneObjects()
        {
            Debug.Log("[UIRootDetection.cs] Deactivating relevant scene objects.");
        
            sceneObjects.ForEach(item => item.gameObject.SetActive(false));
        }
    
        public new void Awake()
        {
            base.Awake(); // Initialize singleton
        
            DeactivateSceneObjects();
        
            DetectionView.gameObject.SetActive(true);
            DetectionView.Instance.HideView();
        }

    }
}
