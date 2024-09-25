using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionManager : MonoBehaviour
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
            
            timer = 0;
        }

        timer++;
        
    }
}
