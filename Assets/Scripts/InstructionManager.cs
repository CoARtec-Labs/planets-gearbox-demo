using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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

    private List<Instructions.InstructionStep> _steps;
    private int _currentStepID, _maxStepID; 
    private const int MinStepID = 0;


    // Start is called before the first frame update
    private void Start()
    {
        _steps = new List<Instructions.InstructionStep>();

        InitializeSteps();

        SetCurrentStep(_currentStepID);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void InitializeSteps()
    {
        _steps.Clear();
        
        // Insert instruction steps here
        AddStep("Start", "ring", "Add ring");
        AddStep("Step-sun", "sun", "Add sun");
        AddStep("Step-planet1", "planet1", "Add planet1");
        AddStep("Step-planet2", "planet2", "Add planet2");
        AddStep("Step-planet3", "planet3", "Add planet3");
        AddStep("Step-carrier", "carrier", "Add carrier");
        AddStep("Step-gasket", "gasket", "Add gasket");
        AddStep("Step-lid", "lid", "Add lid");
        
        _maxStepID = _steps.Count-1;

        _currentStepID = 0;
    }

    private void AddStep(string stepName, string modelName, string description)
    {
        // Find mesh model. Assume that we are on the same level as Origin (root of GameObject Assembly)
        var model = transform.parent.Find("Origin/" + modelName).gameObject;
        
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
                _steps[i].Activate();
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