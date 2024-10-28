using System;
using System.Collections.Generic;

namespace _Scripts.Services.Storage
{
    public interface ILoadStorageService : IService
    {
        void LoadLines(string fileName, Action<List<string>> completedCallback);
        T LoadPrefs<T>(string key, T defaultValue);
    }
}