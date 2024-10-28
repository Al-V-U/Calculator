namespace _Scripts.Services.Calculator
{
    public interface ICalculatorService : IService
    {
        (string, bool) AddCalculate(string equation);
    }
}