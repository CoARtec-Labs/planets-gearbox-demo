using UI.Data;
using UnityEngine;

namespace UI.Views
{
    public class UIRootBase : MonoBehaviour, IStateData
    {
        // Functionality common to all UIRoot classes.
        public static int SearchObjectClassId { get; set; }
    }
}