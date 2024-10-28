using System.Collections.Generic;

namespace _Scripts.UI.CalculatorScreen
{
    public interface ICalculatorView
    {
        void SetInputFieldText(string text);
        void HistoryUpdated();
        void SetHistory(IReadOnlyList<string> history);
        void SetActive(bool isActive);
    }
}