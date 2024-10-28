namespace _Scripts.Services
{
    public class AllServices
    {
        private static class Instance<TService> where TService : IService
        {
            public static TService Service;
        }

        private static AllServices _instance;

        public static AllServices Container => 
                _instance ??= new AllServices();

        public void Register<T>(T serviceInstance) where T : IService => 
            Instance<T>.Service = serviceInstance;

        public T Get<T>() where T : IService => 
            Instance<T>.Service;
    }
}