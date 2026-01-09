using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;

namespace Instructions
{
    /// <summary>
    /// Script to handle events of the AR image detector and tracker like adding, updating and removing. 
    /// </summary>
    public class ARTrackedImagesActions : MonoBehaviour
    {
        [SerializeField] private ARTrackedImageManager imageTracker;
        [SerializeField] private GameObject partsAssembly;

        private void Start()
        {
            // void OnEnable() => arTrackedImageManagerRef.trackedImagesChanged += OnChanged;
            //
            // void OnDisable() => arTrackedImageManagerRef.trackedImagesChanged -= OnChanged;

            // Enable handling of on-change events of tracked images
            imageTracker.trackedImagesChanged += OnChanged;
        }

        void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            // New iamges
            foreach (var newImage in eventArgs.added)
            {
                // Handle added event
                foreach (var trackedImage in imageTracker.trackables)
                {
                    Debug.Log($"New tracked image: {trackedImage.referenceImage.name} at " +
                              $"{trackedImage.transform.position} with ID " +
                              $"{trackedImage.trackableId}");
                }
                
                // Stick assembly to tracked image
                // assemblyRef.transform.parent = newImage.transform;
                // assemblyRef.transform.localPosition = new Vector3(0, 0, 0);
                // assemblyRef.transform.localRotation = Quaternion.identity;
                
                // Copy image global position
                // assemblyRef.transform.position = newImage.transform.position;
                // assemblyRef.transform.rotation = newImage.transform.rotation;
                
                Instantiate(partsAssembly, newImage.transform);
                
            }

            // Updated images
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
                partsAssembly.transform.position = updatedImage.transform.position;
                partsAssembly.transform.rotation = updatedImage.transform.rotation;
                
            }

            // Removed image
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