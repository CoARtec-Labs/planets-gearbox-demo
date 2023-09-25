using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class ButtonAction : MonoBehaviour
{

    public GameObject targetButton;
    public UnityEvent action;

    private Button buttonButton;

    void Start()
    {


        //targetButton = GameObject.Find("Button_next");
        buttonButton = targetButton.GetComponent<Button>();

        if (buttonButton == null)
        {
            Debug.Log("Button not found");
        }
        else
        {
            Debug.Log("Button found: " + buttonButton.gameObject.name);
            buttonButton.onClick.AddListener(UserClick);
        }
    }


    void UserClick() 
    {
        Debug.Log(targetButton.name + " was clicked");
    }


}
