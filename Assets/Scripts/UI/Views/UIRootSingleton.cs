using UnityEngine;

namespace UI.Views
{
    /// <summary>
    /// Template class providing the singleton pattern and (de)activation for UI views.
    /// </summary>
    public class UIRootSingleton<T> : UIRootBase where T : Component
    {
        public static T Instance {get; private set;}

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;

                // This one only works on root GameObjects. It is not
                // really necessary for our View objects anyways.
                //DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }
    }
}
