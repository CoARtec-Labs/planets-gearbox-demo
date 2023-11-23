using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Instructions.Data;
using Instructions.AndroidPlatform;

namespace Instructions.AndroidPlatform
{
    public static class AndroidPlatformManager
    {
        private const string ANDROID_UNITY_BRIDGE_FIELD = "androidUnityBridge";
        private const string CURRENT_STEP_INDEX_ANDROID_JAVA_FIELD_SETTER = "setCurrentStepIndex";

        private static AndroidJavaObject _androidUnityBridge;

        public static AndroidJavaObject AndroidUnityBridge {
            get { return _androidUnityBridge; } 
            private set { _androidUnityBridge = value; }
        }


        public static InstructionData CurrentInstruction { get; private set; }
        
        private static GetInstruction _getInstruction;
        private static ButtonCallbacks _buttonCallbacks;

        static AndroidPlatformManager()
        {
            Initialize();
            Debug.Log("AndroidDataBridge");
            ReceiveInstruction(  instruction =>
            {
                Debug.Log($"AndroidPlatformManager: Received instruction={instruction.ToString()}");
                CurrentInstruction = instruction;
            }
            );
            

        }

        private static void Initialize()
        {
            InitializeAndroidUnityBridge();
            InitializeGetInstruction();
            InitializeButtonCallbacks();
        }

        public static void InitializeAndroidUnityBridge() {


            var androidJavaUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var currentActivity = androidJavaUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    
            AndroidUnityBridge = currentActivity.Get<AndroidJavaObject>(ANDROID_UNITY_BRIDGE_FIELD);

            Debug.Log("AndroidPlatformManager: AndroidUnityBridge initialized!");


        }

        private static void InitializeGetInstruction()
        {
            _getInstruction = new GetInstruction();
            Debug.Log("AndroidPlatformManager: GetInstruction initialized!");

        }

        private static void InitializeButtonCallbacks()
        {

            _buttonCallbacks = new ButtonCallbacks();
            Debug.Log("AndroidPlatformManager: ButtonCallbacks initialized!");
        }

        public static void ReceiveInstruction(Action<InstructionData> callback)
        {
            if (CurrentInstruction != null)
            {
                callback.Invoke(CurrentInstruction);
                return;
            }
            _getInstruction.Invoke(instruction => 
            {
                CurrentInstruction = instruction;
                callback.Invoke(instruction);
            });
            


        }

        public static void BindButtonHandlers(Action handleStepNext, Action handleStepBack)
        {
            Debug.Log($"AndroidPlatformManager: BindButtonHandlers: handleStepNext={handleStepNext}, handleStepBack={handleStepBack}!");

            
            _buttonCallbacks.BindButtonHandlers(handleStepNext, handleStepBack);
        }

        public static void UpdateCurrentStepIndex(int index)
        {
            Debug.Log($"AndroidPlatformManager: UpdateCurrentStepIndex: index={index}!");


            _androidUnityBridge.Call(CURRENT_STEP_INDEX_ANDROID_JAVA_FIELD_SETTER, index);
        }



        public static void Destroy()
        {
            Debug.Log($"AndroidPlatformManager: Destroy: Called!");

            _getInstruction.cancel();

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

    public class GetInstruction : AndroidJavaProxy
    {

        private const string DATA_RECEIVED_CALLBACK_ANDROID_JAVA_OBJECT = "de.sebiger.arinstructions.domain.ar.callbacks.DataReceivedCallback";
        private const string GET_INSTRUCTION_ANDROID_JAVA_METHOD = "getInstruction";

        private const string CANCEL_ANDROID_JAVA_METHOD = "cancel";
        private static Action<InstructionData> dataReceivedCallback = null;
        public GetInstruction() : base(DATA_RECEIVED_CALLBACK_ANDROID_JAVA_OBJECT)
        {
        }

        

        public void OnDataReceived(string instructionJSON)
        {

            Debug.Log($"GetInstructionSteps: OnDataReceived: JSON = {instructionJSON}");
            if (dataReceivedCallback != null)
            {
                var instruction = InstructionData.CreateFromJson(instructionJSON);
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
            AndroidPlatformManager.AndroidUnityBridge.Call(GET_INSTRUCTION_ANDROID_JAVA_METHOD, this);


        }

        public void cancel()
        {
            dataReceivedCallback = null;
            AndroidPlatformManager.AndroidUnityBridge.Call(CANCEL_ANDROID_JAVA_METHOD);

        }
    }

    interface DataObserver<T>
    {
        void OnValueChange(T value);
    }

    class ButtonCallbacks : AndroidJavaProxy
    {

        private const string ANDROID_JAVY_OBJECT_NAME = "de.sebiger.arinstructions.domain.ar.callbacks.UnityButtonCallbacks";
        private const string INITIALIZE_BUTTON_CALLBACKS_ANDROID_JAVA_METHOD = "initButtonCallbacks";

        private static Action _handleStepNext;

        private static Action _handleStepBack;


        public ButtonCallbacks() : base(ANDROID_JAVY_OBJECT_NAME)
        {
            Initialize();
        }

        private void Initialize()
        {
            AndroidPlatformManager.AndroidUnityBridge.Call(INITIALIZE_BUTTON_CALLBACKS_ANDROID_JAVA_METHOD, this);

            Debug.Log("ButtonCallbacks initialized!");

        }

        public void BindButtonHandlers(Action handleStepNext, Action handleStepBack)
        {
            Debug.Log($"ButtonCallbacks: BindButtonHandlers: handleStepNext={handleStepNext}, handleStepBack={handleStepBack}!");


            _handleStepNext = handleStepNext;
            _handleStepBack = handleStepBack;
        }

        void OnStepNext() 
        {
            Debug.Log("ButtonCallbacks: OnStepNext");
            if (_handleStepNext != null)
            {
                _handleStepNext.Invoke();
            }
            else
            {
                Debug.Log("ButtonCallbacks: _handleStepNext: null");
            }

        }
        void OnStepBack()
        {
            Debug.Log("ButtonCallbacks: OnStepBack");

            if (_handleStepBack != null)
            {
                _handleStepBack.Invoke();
            } 
            else
            {
                Debug.Log("ButtonCallbacks: _handleStepBack: null");
            }
        }

    }


}

