using System;

using UnityEngine;

public class DummyScript : MonoBehaviour
{

    void Start()
    {
    }


    public void Dummy(string text)
    {
        Debug.Log($"DUMMY {text}");
    }
}