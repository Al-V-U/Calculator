namespace _Scripts.Services.Calculator
{
    public interface ICalculateService : IService
    {
        (string, bool) AddCalculate(string equation);
    }
}