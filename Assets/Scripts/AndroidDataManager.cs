using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Instructions.Data;

namespace Instructions.AndroidPlatform
{
    public class AndroidDataManager
    {
        public InstructionData CurrentInstruction { get; private set; }
        
        private GetInstruction getInstruction = new GetInstruction();

        public AndroidDataManager()
        {
            Debug.Log("AndroidDataBridge");
            receiveInstruction(  instruction =>
            {
                Debug.Log($"AndroidDataManager: Received instruction={instruction.ToString()}");
                CurrentInstruction = instruction;
            }
            );

        }

        public void receiveInstruction(Action<InstructionData> callback)
        {
            if (CurrentInstruction != null)
            {
                callback.Invoke(CurrentInstruction);
                return;
            }
            getInstruction.Invoke(instruction => 
            {
                CurrentInstruction = instruction;
                callback.Invoke(instruction);
            });

        }



        public void Destroy()
        {
            getInstruction.cancel();
        }

        
        // // Start is called before the first frame update
        // void Start()
        // {
            
        // }

        // // Update is called once per frame
        // void Update()
        // {
            
        // }
    }

    class GetInstruction : AndroidJavaProxy
    {

 
        private Action<InstructionData> dataReceivedCallback = null;
        private AndroidJavaObject androidGetInstructionsBridge;
        public GetInstruction() : base("de.sebiger.arinstructions.DataReceivedCallback")
        {
            androidGetInstructionsBridge = new AndroidJavaObject("de.sebiger.arinstructions.GetInstructionsBridge", this);
            Debug.Log("GetInstructionSteps initialized");
        }

        public void OnDataReceived(string instructionJSON)
        {

            Debug.Log($"GetInstructionSteps: OnDataReceived: JSON = {instructionJSON}");
            if (dataReceivedCallback != null)
            {
                InstructionData instruction = InstructionData.CreateFromJson(instructionJSON);
                dataReceivedCallback(instruction);
            }
        }

        public void Invoke(Action<InstructionData> callback)
        {
            if (dataReceivedCallback != null)
            {
                cancel();
            }
            dataReceivedCallback = callback;
            androidGetInstructionsBridge.Call("getInstruction");

        }

        public void cancel()
        {
            dataReceivedCallback = null;
            androidGetInstructionsBridge.Call("cancel");
        }
    }

    interface DataObserver<T>
    {
        void OnValueChange(T value);
    }


}

