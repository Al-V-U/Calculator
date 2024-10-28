using System;
using _Scripts.Services;
using _Scripts.Services.Calculator;
using _Scripts.Services.Storage;
using _Scripts.UI.CalculatorScreen;
using _Scripts.UI.ErrorScreen;
using UnityEngine;


namespace _Scripts
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private Transform screenParent;
        
        [SerializeField] private CalculatorView calculatorScreenPrefab;
        [SerializeField] private ErrorView errorScreenPrefab;
        
        private void Awake()
        {
            RegisterServices();
            ICalculator calculator = CreateCalculatorScreen();
            LoadHistory(calculator);
        }

        private void RegisterServices()
        {
            StorageService storageService = new StorageService();
            AllServices.Container.Register<ISaveStorageService>(storageService);
            AllServices.Container.Register<ILoadStorageService>(storageService);
            AllServices.Container.Register<ICalculatorService>(new CalculatorService());
        }

        private void LoadHistory(ICalculator calculator)
        {
            ILoadStorageService loadStorageService = AllServices.Container.Get<ILoadStorageService>();
            string inputFieldText = loadStorageService.LoadPrefs(GameConst.InputFieldText, ""); 
            calculator.SetInputFieldText(inputFieldText);
            
            loadStorageService.LoadLines(GameConst.HistoryFileName, calculator.HistoryLoaded);
        }

        private ICalculator CreateCalculatorScreen()
        {
            CalculatorView view = Instantiate(calculatorScreenPrefab, screenParent);
            
            CalculatorModel model = new CalculatorModel(view, AllServices.Container.Get<ISaveStorageService>(), GameConst.HistoryFileName);
            
            CalculatorPresenter presenter = new CalculatorPresenter(model, AllServices.Container.Get<ICalculatorService>(), ShowErrorScreen);
            
            view.Construct(presenter);
            
            return presenter;
        }

        private void ShowErrorScreen(Action closeCallback)
        {
            ErrorView errorView = Instantiate(errorScreenPrefab, screenParent);
            errorView.CloseCallback = closeCallback;
        }
    }
}