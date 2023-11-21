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

        public List<InstructionStep> instructionSteps;

        public static InstructionData CreateFromJson(string json)
        {
            return JsonUtility.FromJson<InstructionData>(json);

        }

    }

    // Represents one stage of the instruction process.
    // It holds reference to the model, which is handled at this step.
    [Serializable]
    public class InstructionStep
    {
        public string stepName;
        public string modelName; 
        public string modelDescription;
        public string stepDescription;
        public string gameObjectName; // use to find model reference

        private GameObject model; // model reference

        public void activate()
        {
            // find and bring up model
            if (model == null || model.name != gameObjectName) {
                model = findModel(gameObjectName);
            }
            
            MeshRenderer[] renderers = model.GetComponentsInChildren<MeshRenderer>();
            
            foreach (MeshRenderer renderer in renderers)
            {
                renderer.enabled = true;                
            }
        }

        public void deactivate()
        {
            // find and hide model
            if (model == null || model.name != gameObjectName) {
                model = findModel(gameObjectName);
            }

            MeshRenderer[] renderers = model.GetComponentsInChildren<MeshRenderer>();
            
            foreach (MeshRenderer renderer in renderers)
            {
                renderer.enabled = false;
            }
        }

        private GameObject findModel(string name)
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
