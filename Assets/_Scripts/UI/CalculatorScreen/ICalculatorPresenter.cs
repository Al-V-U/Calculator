using System;

namespace _Scripts.UI.CalculatorScreen
{
    public interface ICalculatorPresenter
    {
        void Calculate(string equation, Action<bool> callback);
        void UpdateInputField(string text);
    }
}