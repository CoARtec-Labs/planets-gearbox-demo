using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Instructions.AndroidPlatform;
using Instructions.Data;

// Defines ordered list of steps and coordinates transitions between them.
// TODO Implement as Singelton
public class InstructionManager : MonoBehaviour
{

    private List<Instructions.InstructionStep> steps;
    private int currentStepID, maxStepID; 
    private const int minStepID = 0;

    private AndroidDataManager androidDataManager;



    // Start is called before the first frame update
    void Start()
    {
        steps = new List<InstructionStep>();

        // initialize instructionSteps For Android: 
        #if UNITY_ANDROID && !UNITY_EDITOR
        Debug.Log("Android");
        androidDataManager = new AndroidDataManager();
        androidDataManager.receiveInstruction(instruction =>
        {
            Debug.Log($"InstructionManager: instruction received: {instruction}");
            initializeSteps(instruction.instructionSteps);
            setCurrentStep(currentStepID);

        });
        #endif

        #if UNITY_EDITOR
       // InstructionData instruction = JsonUtility.FromJson<InstructionData>("{\"assemblyId\":1,\"assemblyName\":\"Planetengetriebe\",\"instructionSteps\":[{\"id\":1,\"stepName\":\"Start\",\"modelName\":\"ring\"},{\"id\":2,\"stepName\":\"Step-sun\",\"modelName\":\"sun\"},{\"id\":3,\"stepName\":\"Step-planet1\",\"modelName\":\"planet1\"},{\"id\":4,\"stepName\":\"Step-planet2\",\"modelName\":\"planet2\"},{\"id\":5,\"stepName\":\"Step-planet3\",\"modelName\":\"planet3\"},{\"id\":6,\"stepName\":\"Step-carrier\",\"modelName\":\"carrier\"},{\"id\":7,\"stepName\":\"Step-gasket\",\"modelName\":\"gasket\"},{\"id\":8,\"stepName\":\"Step-lid\",\"modelName\":\"lid\"}]}");
       // initializeSteps(instruction.instructionSteps);
       initializeSteps();
        setCurrentStep(currentStepID);

        #endif

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void initializeSteps(List<InstructionStep> newSteps)
    {
        steps = newSteps;
        maxStepID = steps.Count-1;
        currentStepID = 0;
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

        initializeSteps(steps);
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
            if (i <= stepID)
            {
                steps[i].activate();
            }
            else
            {
                steps[i].deactivate();
            }
        }
    }
}