using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionUpdater : MonoBehaviour
{
    public ObjectDetector objectDetector;
    public int updateRateMultiplier = 0;

    private int timer = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if( timer % updateRateMultiplier == 0)
        {
            objectDetector.RunPrediction_trigger();
            //objectDetector.BBoxArrayTest();

            timer = 0;
        }

        timer++;
        
    }
}
