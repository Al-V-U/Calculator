using System.Collections.Generic;
using _Scripts.Services.Storage;
using UnityEngine;

namespace _Scripts.UI.CalculatorScreen
{
    public class CalculatorModel
    {
        private readonly ICalculatorView _view;
        
        private List<string> _history = new();
        
        private readonly ISaveStorageService _saveStorageService;
        
        private readonly string _saveFileName;

        public CalculatorModel(ICalculatorView view, ISaveStorageService saveStorageService, string saveFileName)
        {
            _view = view;
            _saveStorageService = saveStorageService;
            _saveFileName = saveFileName;
            
            _view.SetHistory(_history);
        }

        public void HistoryLoaded(List<string> history)
        {
            history.AddRange(_history);
            _history = history;
            _view.SetHistory(_history);
        }

        public void AddEquation(string equation)
        {
            _history.Add(equation);
            _view.HistoryUpdated();
            
            _saveStorageService.SaveLine(_saveFileName, equation);
        }

        public void SaveInputField(string text) =>
            _saveStorageService.SavePrefs("input_field_text", text);

        public void SetInputField(string text) =>
            _view.SetInputFieldText(text);

        public void ActivateView() => 
            _view.SetActive(true);
    }
}
