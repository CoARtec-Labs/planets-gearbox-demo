using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Instructions
{

    // Represents one stage of the instruction process.
    // It holds reference to the model, which is handled at this step.
    public class InstructionStep
    {
        public string stepName {get; set;}
        public string modelName {get; set;} // use to find model reference

        private GameObject model; // model reference

        public void activate()
        {
            // find and bring up model
            model = findModel(modelName);
            
            MeshRenderer[] renderers = model.GetComponentsInChildren<MeshRenderer>();
            
            foreach (MeshRenderer renderer in renderers)
            {
                renderer.enabled = true;                
            }
        }

        public void deactivate()
        {
            // find and hide model
            model = findModel(modelName);

            MeshRenderer[] renderers = model.GetComponentsInChildren<MeshRenderer>();
            
            foreach (MeshRenderer renderer in renderers)
            {
                renderer.enabled = false;
            }
        }

        public void SetMaterial(Material mat)
        {
            // find and hide model
            model = findModel(modelName);

            MeshRenderer[] renderers = model.GetComponentsInChildren<MeshRenderer>();
            
            foreach (MeshRenderer renderer in renderers)
            {
                renderer.material = mat;
            }
        }

        GameObject findModel(string name)
        {
            model = GameObject.Find(name);

            if (model == null)
            {
                throw new Exception($"Could not find model {name}");
            }
            else
            {
                return model;
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

    private List<Instructions.InstructionStep> steps;
    private int currentStepID, maxStepID; 
    private const int minStepID = 0;


    // Start is called before the first frame update
    void Start()
    {
        steps = new List<Instructions.InstructionStep>();

        initializeSteps();

        setCurrentStep(currentStepID);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void initializeSteps()
    {
        steps.Clear();

        // Insert instruction steps here
        steps.Add(new Instructions.InstructionStep() { stepName = "Start", modelName = "ring" });
        steps.Add(new Instructions.InstructionStep() { stepName = "Step-sun", modelName = "sun" });
        steps.Add(new Instructions.InstructionStep() { stepName = "Step-planet1", modelName = "planet1" });
        steps.Add(new Instructions.InstructionStep() { stepName = "Step-planet2", modelName = "planet2" });
        steps.Add(new Instructions.InstructionStep() { stepName = "Step-planet3", modelName = "planet3" });
        steps.Add(new Instructions.InstructionStep() { stepName = "Step-lid", modelName = "carrier" });
        steps.Add(new Instructions.InstructionStep() { stepName = "Step-lid", modelName = "gasket" });
        steps.Add(new Instructions.InstructionStep() { stepName = "Step-lid", modelName = "lid" });

        maxStepID = steps.Count-1;

        currentStepID = 0;
    }

    public void stepNext()
    {
        System.Diagnostics.Debug.Assert(currentStepID >= minStepID && currentStepID <= maxStepID);

        if (currentStepID < maxStepID)
        {
            currentStepID ++;
            setCurrentStep(currentStepID);
        }
        else
        {
            Debug.Log("Last step was already reached");
        }
    }

    public void stepBack()
    {
        System.Diagnostics.Debug.Assert(currentStepID >= minStepID && currentStepID <= maxStepID);

        if (currentStepID > minStepID)
        {
            currentStepID --;
            setCurrentStep(currentStepID);
        }
        else
        {
            Debug.Log("First step was already reached");
        }
    }

    // Coordinates states of each step for given step ID
    private void setCurrentStep(int stepID)
    {
        for (int i = minStepID; i <= maxStepID; i++)
        {   
            if (i < stepID)
            {
                steps[i].activate();
                steps[i].SetMaterial(materialInactiveStep);
            }
            else if (i == stepID)
            {
                steps[i].activate();
                steps[i].SetMaterial(materialActiveStep);
            }
            else
            {
                steps[i].deactivate();
                steps[i].SetMaterial(materialInactiveStep);
            }
        }
    }
}