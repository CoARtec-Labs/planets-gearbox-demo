using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayTextAction : MonoBehaviour
{

    private TMP_Text _textRef;
    private string _text = "";
    string Text
    {
        get
        {
            return _text;
        }
        set
        {
            _text = value;
            DisplayText(value);
        }
    }

    
    void Awake()
    {
        Debug.Log("DisplayTextAction: Awake");
    }

    void Start()
    {
        _textRef = this.gameObject.GetComponent<TMP_Text>();
        
        Debug.Log("DisplayTextAction: Start");

        
    }

    private void DisplayText(string text)
    {
        if (_textRef != null)
        {
            _textRef.SetText(text);

        }
    }

    
}
