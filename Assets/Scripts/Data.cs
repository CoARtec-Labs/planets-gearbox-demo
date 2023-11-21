using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Instructions.Data
{
    [Serializable]
    public class InstructionData
    {
        public string assemblyId;

        public string assemblyName;

        public List<InstructionStepData> instructionSteps;

        public static InstructionData CreateFromJson(string json)
        {
            return JsonUtility.FromJson<InstructionData>(json);

        }

    }

    // Represents one stage of the instruction process.
    // It holds reference to the model, which is handled at this step.
    [Serializable]
    public class InstructionStepData
    {
        public string stepName;
        public string modelName; 
        public string modelDescription;
        public string stepDescription;
        public string gameObjectName; // use to find model reference

    }


}
