using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Services.Storage
{
    public class StorageService : ISaveStorageService, ILoadStorageService
    {
        private bool _isLoading;
        private bool _isSaving;
        
        private readonly List<string> _needToSaveLines = new();
        

        public void SaveLine(string fileName, string line, Action<bool> completedCallback = null) => 
            SaveLineAsync(fileName, line, completedCallback);

        public void LoadLines(string fileName, Action<List<string>> completedCallback) => 
            LoadLinesAsync(fileName, completedCallback);

        private async void SaveLineAsync(string fileName, string line, Action<bool> completedCallback)
        {
            string path = GetPath(fileName);
            _needToSaveLines.Add(line);
            if (_isLoading)
            {
                completedCallback?.Invoke(false);
                await WaitForFalse(() => _isLoading);
            }

            if (_isSaving)
            {
                completedCallback?.Invoke(false);
                await WaitForFalse(() => _isSaving);
            }

            if (_needToSaveLines.Count == 0)
            {
                completedCallback?.Invoke(false);
                return;
            }
            
            _isSaving = true;

            List<string> saveLines = new List<string>(_needToSaveLines);
            _needToSaveLines.Clear();
            
            await using (StreamWriter sw = new StreamWriter(path, true)) 
            {
                foreach (var savedLine in saveLines)
                    await sw.WriteLineAsync(savedLine);
            }

            _isSaving = false;
            completedCallback?.Invoke(_needToSaveLines.Count == 0);
        }
        
        private async void LoadLinesAsync(string fileName, Action<List<string>> completedCallback)
        {
            List<string> lines = new List<string>();

            string path = GetPath(fileName);
            if (File.Exists(path))
            {
                _isLoading = true;

                using (StreamReader sr = new StreamReader(path))
                {
                    while (await sr.ReadLineAsync() is { } line)
                    {
                        lines.Add(line);
                    }
                }
                _isLoading = false;
            }

            completedCallback.Invoke(lines);
        }

        public void SavePrefs<T>(string key, T value)
        {
            if (typeof(T) == typeof(int))
            {
                PlayerPrefs.SetInt(key, (int)(object)value);
            }
            else if (typeof(T) == typeof(string))
            {
                PlayerPrefs.SetString(key, (string)(object)value);
            }
            else
            {
                throw new ArgumentException("T must be either int or string.");
            }
        }

        public T LoadPrefs<T>(string key, T defaultValue)
        {
            if (typeof(T) == typeof(int))
            {
                return (T)(object)PlayerPrefs.GetInt(key, (int)(object)defaultValue);
            } 
            if (typeof(T) == typeof(string))
            {
                return (T)(object)PlayerPrefs.GetString(key, (string)(object)defaultValue);
            }
            
            throw new ArgumentException("T must be either int or string.");
        }

        private static string GetPath(string fileName) => 
            Path.Combine(Application.persistentDataPath, fileName);
        
        private async Task WaitForFalse(Func<bool> condition)
        {
            while (condition())
            {
                await Task.Yield();
            }
        }
    }
}