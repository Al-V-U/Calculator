using System;
using System.Collections.Generic;
using _Scripts.Services.Calculator;
using UnityEngine;

namespace _Scripts.UI.CalculatorScreen
{
    public class CalculatorPresenter : ICalculatorPresenter, ICalculator
    {
        private readonly CalculatorModel _model;
        private readonly ICalculateService calculateService;
        private readonly Action<Action> _showErrorScreen;

        public CalculatorPresenter(CalculatorModel model, ICalculateService calculateService, Action<Action> showErrorScreen)
        {
            _model = model;
            this.calculateService = calculateService;
            _showErrorScreen = showErrorScreen;
        }
        
        public void Calculate(string equation, Action<bool> callback)
        {
            var addCalculate = calculateService.AddCalculate(equation);
            
            _model.AddEquation(addCalculate.Item1);
            
            if (!addCalculate.Item2)
                _showErrorScreen?.Invoke(_model.ActivateView);
            
            callback(addCalculate.Item2);
        }
        
        public void UpdateInputField(string text) =>
            _model.SaveInputField(text);

        public void SetInputFieldText(string text) => 
            _model.SetInputField(text);

        public void HistoryLoaded(List<string> history) =>
            _model.HistoryLoaded(history);
    }
}
