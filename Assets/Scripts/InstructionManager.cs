using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Instructions.AndroidPlatform;
using Instructions.Data;
using Instructions;

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

        public static InstructionStep From(InstructionStepData data, Transform transform)
        {
            // Find mesh model. Assume that we are on the same level as Origin (root of GameObject Assembly)
            var gameObject = transform.parent.Find("TagRelative/Origin/" + data.gameObjectName).gameObject;

            return new InstructionStep(data.gameObjectName, gameObject) 
            {
                StepName = data.stepName, StepDescription = data.stepDescription
            };

        }

        public static List<InstructionStep> From(List<InstructionStepData> dataList, Transform transform)
        {
            var result = new List<InstructionStep>();
            foreach (var data in dataList)
            {
                result.Add(InstructionStep.From(data, transform));
            }
            return result;
            
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
    private string _currentAssemblyName;



    // Start is called before the first frame update
    private void Start()
    {
        _steps = new List<InstructionStep>();

        // initialize instructionSteps For Android: 
        #if UNITY_ANDROID && !UNITY_EDITOR
        UpdateCurrentStepIndex();
        Debug.Log("Android");
        AndroidPlatformManager.ReceiveInstruction(instruction =>
        {
            Debug.Log($"InstructionManager: instruction received: {instruction}");

            InitializeSteps(InstructionStep.From(instruction.instructionSteps, transform));
            SetCurrentStep(_currentStepID);
            AndroidPlatformManager.BindButtonHandlers(StepNext, StepBack);
        });
        #endif

        #if !UNITY_ANDROID
       // InstructionData instruction = JsonUtility.FromJson<InstructionData>("{\"assemblyId\":1,\"assemblyName\":\"Planetengetriebe\",\"instructionSteps\":[{\"id\":1,\"stepName\":\"Start\",\"modelName\":\"ring\"},{\"id\":2,\"stepName\":\"Step-sun\",\"modelName\":\"sun\"},{\"id\":3,\"stepName\":\"Step-planet1\",\"modelName\":\"planet1\"},{\"id\":4,\"stepName\":\"Step-planet2\",\"modelName\":\"planet2\"},{\"id\":5,\"stepName\":\"Step-planet3\",\"modelName\":\"planet3\"},{\"id\":6,\"stepName\":\"Step-carrier\",\"modelName\":\"carrier\"},{\"id\":7,\"stepName\":\"Step-gasket\",\"modelName\":\"gasket\"},{\"id\":8,\"stepName\":\"Step-lid\",\"modelName\":\"lid\"}]}");
       // initializeSteps(instruction.instructionSteps);
       InitializeSteps();
        SetCurrentStep(_currentStepID);

        #endif

    }



    // Update is called once per frame
    private void Update()
    {
        
    }

    private void InitializeSteps(List<InstructionStep> newSteps)
    {
        _steps = newSteps;
        _maxStepID = _steps.Count-1;
        _currentStepID = 0;
        Debug.Log("Initialized Steps");
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
        

        InitializeSteps(_steps);
    }

    private void AddStep(string stepName, string gameObjectName, string stepDescription = null, string modelName = null, string modelDescription = null)
    {
        // Find mesh model. Assume that we are on the same level as Origin (root of GameObject Assembly)
        var model = transform.parent.Find("TagRelative/Origin/" + gameObjectName).gameObject;
        
        _steps.Add(new Instructions.InstructionStep(gameObjectName, model) 
        { 
            StepName = stepName,
            StepDescription = stepDescription,
            ModelName = modelName
            
            });
    }

    /**
    * Function to bind to Unity StepNext button.
    * Sets the next instructionstep.
    */
    public void StepNext()
    {
        Debug.Log("InstructionManager: OnStepNextClicked");

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
        Debug.Log("InstructionManager: OnStepBackClicked");
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
        var currentStep = stepID;
        Debug.Log($"InstructionManager: SetCurrentStep={currentStep}");

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
                try
                {
                    titleTextRef.SetText(_steps[i].StepName);
                    descriptionTextRef.SetText(_steps[i].StepDescription);
                }
                catch(Exception exception)
                {
                    Debug.Log($"InstructionManager: SetCurrentStep: exception={exception.Message}");
                }
            }
            else
            {
                _steps[i].Deactivate();
                _steps[i].SetMaterial(materialInactiveStep);
            }
        }
            #if UNITY_ANDROID && !UNITY_EDITOR
            UpdateCurrentStepIndex();
            #endif


    }

    #if UNITY_ANDROID && !UNITY_EDITOR
    private void UpdateCurrentStepIndex()
    {
        Debug.Log($"InstructionManager: UpdateCurrentStepIndex={_currentStepID}");

        AndroidPlatformManager.UpdateCurrentStepIndex(_currentStepID);
    }
    #endif
}