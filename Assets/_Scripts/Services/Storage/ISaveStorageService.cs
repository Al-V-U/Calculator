using System;

namespace _Scripts.Services.Storage
{
    public interface ISaveStorageService : IService
    {
        void SaveLine(string fileName, string line, Action<bool> completedCallback = null);
        void SavePrefs<T>(string key, T value);
    }
}