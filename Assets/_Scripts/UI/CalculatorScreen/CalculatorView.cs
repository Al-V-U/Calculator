using System.Collections;
using System.Collections.Generic;
using _Scripts.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.CalculatorScreen
{
    public class CalculatorView : MonoBehaviour, ICalculatorView
    {
        [SerializeField] private Button calculateButton;
        
        [SerializeField] private TMP_InputField inputField;

        [SerializeField] private HistoryResults historyResults;

        [SerializeField] private Image separator;
        
        [SerializeField] private Color separatorActiveColor;
        
        private ICalculatorPresenter _presenter;

        private bool _isActive = true;
        
        public void Construct(ICalculatorPresenter presenter)
        {
            _presenter = presenter;
        }
        private void Start()
        {
            calculateButton.onClick.AddListener(OnCalculateClick);
            inputField.onValueChanged.AddListener(OnInputFieldUpdated);

            inputField.ActivateInputField();
        }

        private void Update()
        {
            if (_isActive && Input.GetKeyDown(KeyCode.Return))
                OnCalculateClick();
        }

        public void SetInputFieldText(string text) => 
            inputField.text = text;

        public void HistoryUpdated()
        {
            separator.color = separatorActiveColor;
            historyResults.SetResultsHeight();
        }

        public void SetHistory(IReadOnlyList<string> history)
        {
            if(history.Count > 0) 
                separator.color = separatorActiveColor;
            historyResults.SetHistory(history);
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
            inputField.interactable = _isActive;
            calculateButton.interactable = _isActive;
            
            if (_isActive)
                inputField.ActivateInputField();
        }

        private void OnCalculateClick()
        {
            if (!string.IsNullOrWhiteSpace(inputField.text))
            {
                _presenter.Calculate(inputField.text, success =>
                {
                    if (success)
                        SetInputFieldText("");
                    else
                        SetActive(false);
                });
            }
            inputField.ActivateInputField();
        }

        private void OnInputFieldUpdated(string text)
        {
            _presenter.UpdateInputField(text);
            calculateButton.interactable = !string.IsNullOrEmpty(text);
        }
    }
}
