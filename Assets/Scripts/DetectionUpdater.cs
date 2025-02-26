using System.Collections;
using System.Collections.Generic;
using coartec;
using UnityEngine;
using UnityEngine.Serialization;

public class DetectionUpdater : MonoBehaviour
{
    public int updateRateMultiplier = 0;

    public DetectionManager detector;

    private int timer = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
        if( timer % updateRateMultiplier == 0)
        {
            //scriptDetector.RunPrediction_trigger();
            //objectDetector.BBoxArrayTest();
            detector.TriggerDetection();

            timer = 0;
        }

        timer++;
        
    }
}
