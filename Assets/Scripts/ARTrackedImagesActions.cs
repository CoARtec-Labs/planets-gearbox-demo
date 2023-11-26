using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;

namespace Instructions
{
    public class ARTrackedImagesActions:MonoBehaviour
    {
        [SerializeField]
        public ARTrackedImageManager arTrackedImageManagerRef;
        public GameObject assemblyRef;

        void OnEnable() => arTrackedImageManagerRef.trackedImagesChanged += OnChanged;
            
        void OnDisable() => arTrackedImageManagerRef.trackedImagesChanged -= OnChanged;

        private void Start()
        {

            /*
            var mARSessionOrigin = GameObject.Find("ARSessionOrigin");
            var managers = mARSessionOrigin.GetComponentsInChildren<ARTrackedImageManager>();

            if (managers.Length > 1)
            {
                throw new Exception("received more than one ARTrackedImageManager");
            }
            else
            {
                arTrackedImageManagerRef = managers[0];
            }
            */
            
            

            // Enable handling of on-change events of tracked images
           // arTrackedImageManagerRef.trackedImagesChanged += OnChanged;
            
        }

        void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            Debug.Log($"ARTrackedImagesActions: OnChanged: assemblyRef={assemblyRef}");

            foreach (var newImage in eventArgs.added)
            {
                // Handle added event
                Debug.Log("Added new image");
                Debug.Log($"ARTrackedImagesActions: newImage: newImage={newImage}");

                foreach (var trackedImage in arTrackedImageManagerRef.trackables)
                {
                    Debug.Log($"Image: {trackedImage.referenceImage.name} is at " +
                              $"{trackedImage.transform.position} with ID " +
                              $"{trackedImage.trackableId}");
                }
                
                // // Spawn new GameObject
                // GameObject newSpawn = Instantiate(spawnObject);

                // Stick assembly to tracked image
                assemblyRef.transform.parent = newImage.transform;
                assemblyRef.transform.localPosition = new Vector3(0, 0, 0);
                assemblyRef.transform.localRotation = Quaternion.identity;

            }

            foreach (var updatedImage in eventArgs.updated)
            {
                Debug.Log($"ARTrackedImagesActions: updatedImage: updatedImage={updatedImage}");

                // Handle updated event
            }

            foreach (var removedImage in eventArgs.removed)
            {
                Debug.Log($"ARTrackedImagesActions: removedImage: removedImage={removedImage}");

                // Handle removed event
            }
        }            
        
        private void Update()
        {
        }
        
        

    }
}