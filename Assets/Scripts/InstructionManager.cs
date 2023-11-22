using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Instructions.AndroidPlatform;
using Instructions.Data;

namespace Instructions
{

    // Represents one stage of the instruction process.
    // It holds reference to the model, which is handled at this step.
    public class InstructionStep
    {
        public string StepName {get; set;}

        public string StepDescription {get; set;}
        public string GameObjectName {get; private set;} // use to find model reference
        public string ModelName { get; set; }
        public string ModelDescription {get; set;}

        private readonly GameObject _model; // mesh model reference
        
        public InstructionStep(
            string gameObjectName,
             GameObject model)
        {
            this._model = model;
            this.GameObjectName = gameObjectName;
        }
        
        public void Activate()
        {
            MeshRenderer[] renderers = _model.GetComponentsInChildren<MeshRenderer>();
            
            foreach (MeshRenderer renderer in renderers)
            {
                renderer.enabled = true;                
            }
        }

        public void Deactivate()
        {
            MeshRenderer[] renderers = _model.GetComponentsInChildren<MeshRenderer>();
            
            foreach (MeshRenderer renderer in renderers)
            {
                renderer.enabled = false;
            }
        }

        public void SetMaterial(Material mat)
        {
            MeshRenderer[] renderers = _model.GetComponentsInChildren<MeshRenderer>();
            
            foreach (MeshRenderer renderer in renderers)
            {
                renderer.material = mat;
            }
        }
    }
}

// Defines ordered list of steps and coordinates transitions between them.
// TODO Implement as Singelton
public class InstructionManager : MonoBehaviour
{
    public Material materialActiveStep;
    public Material materialInactiveStep;

    private List<Instructions.InstructionStep> _steps;
    private int _currentStepID, _maxStepID; 
    private const int MinStepID = 0;
    private string _currentAssemblyName;



    // Start is called before the first frame update
    private void Start()
    {
        _steps = new List<Instructions.InstructionStep>();
        // initialize instructionSteps For Android: 
        #if UNITY_ANDROID && !UNITY_EDITOR
        DestroyUnityButtons();
        Debug.Log("Android");
        AndroidDataManager.ReceiveInstruction(instruction =>
        {
            _currentAssemblyName = instruction.assemblyName;
            Debug.Log($"InstructionManager: instruction received: {instruction}");
            InitializeSteps(instruction.instructionSteps);
            SetCurrentStep(_currentStepID);
            AndroidDataManager.BindButtonHandlers(OnStepNextClicked, OnStepBackClicked);

        });
        #endif

        #if UNITY_EDITOR
       // InstructionData instruction = JsonUtility.FromJson<InstructionData>("{\"assemblyId\":1,\"assemblyName\":\"Planetengetriebe\",\"instructionSteps\":[{\"id\":1,\"stepName\":\"Start\",\"modelName\":\"ring\"},{\"id\":2,\"stepName\":\"Step-sun\",\"modelName\":\"sun\"},{\"id\":3,\"stepName\":\"Step-planet1\",\"modelName\":\"planet1\"},{\"id\":4,\"stepName\":\"Step-planet2\",\"modelName\":\"planet2\"},{\"id\":5,\"stepName\":\"Step-planet3\",\"modelName\":\"planet3\"},{\"id\":6,\"stepName\":\"Step-carrier\",\"modelName\":\"carrier\"},{\"id\":7,\"stepName\":\"Step-gasket\",\"modelName\":\"gasket\"},{\"id\":8,\"stepName\":\"Step-lid\",\"modelName\":\"lid\"}]}");
       // initializeSteps(instruction.instructionSteps);

        InitializeSteps();

        SetCurrentStep(_currentStepID);
       
        #endif

        
    }


    #if UNITY_ANDROID && !UNITY_EDITOR
    /**
    * Destroys Unity buttons because Android App provides its own buttons
    */
    private void DestroyUnityButtons()
    {
        DestroyNextButton();
        DestroyBackButton();

    }

    private void DestroyButton(string buttonName)
    {
        Destroy(transform.parent.Find(buttonName));

    }

    private void DestroyNextButton()
    {
        DestroyButton("Button_next");
    }

    private DestroyBackButton()
    {
        DestroyButton("Button_back");
    }
    #endif



    // Update is called once per frame
    private void Update()
    {
        
    }

    private void InitializeSteps(List<InstructionStepData> stepDataList)
    {
        _steps.Clear();
        foreach (InstructionStepData stepData in stepDataList)
        {
            AddStep(stepData.stepName, stepData.gameObjectName, stepData.stepDescription, stepData.modelName, stepData.modelDescription);
        }
        _maxStepID = _steps.Count-1;
        _currentStepID = 0;
    }

    private void InitializeSteps()
    {
        _steps.Clear();
        
        // Insert instruction steps here
        AddStep("Start", "ring");
        AddStep("Step-sun", "sun");
        AddStep("Step-planet1", "planet1");
        AddStep("Step-planet2", "planet2");
        AddStep("Step-planet3", "planet3");
        AddStep("Step-carrier", "carrier");
        AddStep("Step-gasket", "gasket");
        AddStep("Step-lid", "lid");

        _maxStepID = _steps.Count-1;
        _currentStepID = 0;

    }

    private void AddStep(string stepName, string gameObjectName, string stepDescription = null, string modelName = null, string modelDescription = null)
    {
        // Find mesh model. Assume that we are on the same level as Origin (root of GameObject Assembly)
        var model = transform.parent.Find("Origin/" + gameObjectName).gameObject;
        
        _steps.Add(new Instructions.InstructionStep(gameObjectName, model) 
        { 
            StepName = stepName,
            StepDescription = stepDescription,
            ModelName = modelName
            
            });
    }

    /**
    * Function to bind to Unity StepNext button.
    */
    public void StepNext()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        DestroyNextButton();
        return;
        #endif
        OnStepNextClicked();
    }

    private void OnStepNextClicked()
    {
        System.Diagnostics.Debug.Assert(_currentStepID >= MinStepID && _currentStepID <= _maxStepID);

        if (_currentStepID < _maxStepID)
        {
            _currentStepID ++;
            SetCurrentStep(_currentStepID);
        }
        else
        {
            Debug.Log("Last step was already reached");
        }

    }

    public void StepBack()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        DestroyBackButton();
        return;
        #endif
        OnStepBackClicked();
        
    }

    private void OnStepBackClicked()
    {
        System.Diagnostics.Debug.Assert(_currentStepID >= MinStepID && _currentStepID <= _maxStepID);

        if (_currentStepID > MinStepID)
        {
            _currentStepID --;
            SetCurrentStep(_currentStepID);
        }
        else
        {
            Debug.Log("First step was already reached");
        }
    }

    // Coordinates states of each step for given step ID
    private void SetCurrentStep(int stepID)
    {
        for (int i = MinStepID; i <= _maxStepID; i++)
        {   
            if (i < stepID)
            {
                _steps[i].Activate();
                _steps[i].SetMaterial(materialInactiveStep);
            }
            else if (i == stepID)
            {
                _steps[i].Activate();
                _steps[i].SetMaterial(materialActiveStep);
            }
            else
            {
                _steps[i].Deactivate();
                _steps[i].SetMaterial(materialInactiveStep);
            }
        }
    }
}