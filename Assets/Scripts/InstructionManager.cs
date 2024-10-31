using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.SceneManagement;

namespace Instructions
{

    // Represents one stage of the instruction process.
    // It holds reference to the model, which is handled at this step.
    public class InstructionStep
    {
        public string StepName { get; set; }
        public string ModelName { get; set; } // use to find model reference
        public string Description { get; set; }

        private readonly GameObject _model; // mesh model reference
        
        public InstructionStep(string modelName, GameObject model)
        {
            this._model = model;
            this.ModelName = modelName;
        }
        
        public void Activate()
        {
            MeshRenderer[] renderers = _model.GetComponentsInChildren<MeshRenderer>();
            
            foreach (MeshRenderer renderer in renderers)
            {
                renderer.enabled = true;                
            }
            
            // Fill in text title and descriptions
            
            
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
    public TMP_Text titleTextRef;
    public TMP_Text descriptionTextRef;
    public GameObject assemblyRef;


    private List<Instructions.InstructionStep> _steps;
    private int _currentStepID = 0;
    private int _maxStepID; 
    private const int MinStepID = 0;

    private void Awake()
    {
        _steps = new List<Instructions.InstructionStep>();

        InitializeSteps();

        _currentStepID = StateMachine.currentStepID;

        SetCurrentStep(_currentStepID);

        SceneManager.sceneUnloaded += OnSceneUnloaded;

        //DontDestroyOnLoad(this.gameObject);
    }

    private void OnSceneUnloaded(Scene current)
    {
        Debug.Log("OnSceneUnloaded: " + current);

        StateMachine.currentStepID = _currentStepID;
    }


    // private void onDisable()
    // {
    //     StateMachine.currentStepID = _currentStepID;
    // }


    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void InitializeSteps()
    {
        _steps.Clear();
        
        // Insert instruction steps here
        AddStep("Start", "ring", "Place the gearbox ring onto the base.");
        AddStep("Wheel-sun", "sun", "Place the wheel \"sun\" at the center of the gearbox ring.");
        AddStep("Wheel-planet 1", "planet1", "Clip the first planet between sun and ring at the top right.");
        AddStep("Wheel-planet 2", "planet2", "Clip the second planet between sun and ring at the top left.");
        AddStep("Wheel-planet 3", "planet3", "Clip the third planet between sun and ring at the bottom center.");
        AddStep("Carrier", "carrier", "Place the carrier on top. Make sure that its pins lock at the planets center points.");
        AddStep("Gasket", "gasket", "Align the gasket with the gearbox flange.");
        AddStep("Lid", "lid", "Close the gearbox with the lid.");
        
        _maxStepID = _steps.Count-1;

    }

    private void AddStep(string stepName, string modelName, string description)
    {
        // Find mesh model. Assume that we are on the same level as TagRelative (root of GameObject Assembly)
        var model = assemblyRef.transform.Find("TagRelative/Origin/" + modelName).gameObject;
        
        _steps.Add(new Instructions.InstructionStep(modelName, model) 
            { StepName = stepName, Description = description});
    }

    public void StepNext()
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
                _steps[i].Deactivate();
                _steps[i].SetMaterial(materialInactiveStep);
            }
            else if (i == stepID)
            {
                _steps[i].Activate();
                _steps[i].SetMaterial(materialActiveStep);
                
                // Overwrite current text UI elements
                titleTextRef.SetText(_steps[i].StepName);
                descriptionTextRef.SetText(_steps[i].Description);
            }
            else
            {
                _steps[i].Deactivate();
                _steps[i].SetMaterial(materialInactiveStep);
            }
        }
    }
}