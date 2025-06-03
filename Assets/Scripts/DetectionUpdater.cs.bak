using System.Collections;
using System.Collections.Generic;
using coartec;
using UnityEngine;
using UnityEngine.Serialization;

public class DetectionUpdater : MonoBehaviour
{
    public int updateRateMultiplier = 0;

    public DetectionManager detector;

    private int _timer;
    
    // Start is called before the first frame update
    void Start()
    {
        _timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _timer++;
        
        if( _timer % updateRateMultiplier == 0)
        {
            detector.TriggerDetection();

            _timer = 0;
        }
    }
}
