using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSceneBlankARScene()
    {
        Debug.Log("Loading scene BlankARScene");
        SceneManager.LoadScene(sceneName:"BlankARScene");
    }
    
    public void LoadSceneAssemblySteps()
    {
        Debug.Log("Loading scene AssemblySteps");
        SceneManager.LoadScene(sceneName:"AssemblySteps");
    }

    public void LoadScenePartsDetection()
    {
        Debug.Log("Loading scene PartsDetection");
        SceneManager.LoadScene(sceneName:"PartsDetection");
    }
}
