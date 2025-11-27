using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace CoARtec.UI.Dynamic.Views
{
    public class DynamicAssemblyView : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descriptionText;

        [Header("Events (Buttons)")]
        public UnityEvent OnNextClicked = new UnityEvent();
        public UnityEvent OnBackClicked = new UnityEvent();
        public UnityEvent OnStagingClicked = new UnityEvent();

        public void SetStepText(string title, string description)
        {
            if (titleText) titleText.text = title ?? string.Empty;
            if (descriptionText) descriptionText.text = description ?? string.Empty;
        }

        // connect the functions to the button's OnClick() event in the Inspector
        public void NextClick()    => OnNextClicked?.Invoke();
        public void BackClick()    => OnBackClicked?.Invoke();
        public void StagingClick() => OnStagingClicked?.Invoke();
    }
}
