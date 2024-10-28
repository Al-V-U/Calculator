using System.Collections.Generic;

namespace _Scripts.UI.CalculatorScreen
{
    public interface ICalculator
    {
        void SetInputFieldText(string text);
        void HistoryLoaded(List<string> history);
    }
}