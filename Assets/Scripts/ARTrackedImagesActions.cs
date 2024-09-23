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
        public ARTrackedImageManager arTrackedImageManagerRef;
        public GameObject assemblyRef;

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
            
            // void OnEnable() => arTrackedImageManagerRef.trackedImagesChanged += OnChanged;
            //
            // void OnDisable() => arTrackedImageManagerRef.trackedImagesChanged -= OnChanged;

            // Enable handling of on-change events of tracked images
            arTrackedImageManagerRef.trackedImagesChanged += OnChanged;
            
        }

        void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            foreach (var newImage in eventArgs.added)
            {
                // Handle added event
                Debug.Log("Added new image");
                
                foreach (var trackedImage in arTrackedImageManagerRef.trackables)
                {
                    Debug.Log($"Image: {trackedImage.referenceImage.name} is at " +
                              $"{trackedImage.transform.position} with ID " +
                              $"{trackedImage.trackableId}");
                }
                
                // // Spawn new GameObject
                // GameObject newSpawn = Instantiate(spawnObject);
                
                
                // Stick assembly to tracked image
                // assemblyRef.transform.parent = newImage.transform;
                // assemblyRef.transform.localPosition = new Vector3(0, 0, 0);
                // assemblyRef.transform.localRotation = Quaternion.identity;
                
                // Copy image global position
                // assemblyRef.transform.position = newImage.transform.position;
                // assemblyRef.transform.rotation = newImage.transform.rotation;
                
                Instantiate(assemblyRef, newImage.transform);
                
            }

            foreach (var updatedImage in eventArgs.updated)
            {
                // // Handle updated event
                // Debug.Log("Updated image");
                //
                // foreach (var trackedImage in arTrackedImageManagerRef.trackables)
                // {
                //     Debug.Log($"Image: {trackedImage.referenceImage.name} is at " +
                //               $"{trackedImage.transform.position} with ID " +
                //               $"{trackedImage.trackableId}");
                // }
                
                // Copy image global position
                assemblyRef.transform.position = updatedImage.transform.position;
                assemblyRef.transform.rotation = updatedImage.transform.rotation;
                
            }

            foreach (var removedImage in eventArgs.removed)
            {
                // Handle removed event
            }
        }            
        
        private void Update()
        {
        }
        
        

    }
}