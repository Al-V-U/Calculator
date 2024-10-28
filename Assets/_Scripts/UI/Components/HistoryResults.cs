using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Components
{
    public class HistoryResults : MonoBehaviour
    {
        [SerializeField] private ScrollRect scroll;
        
        [SerializeField] private RectTransform historyParent;

        [SerializeField] private int maxResultElements;
        
        [SerializeField] private int elementSpacing;
        
        [SerializeField] private RectTransform results;
        
        [SerializeField] private ElementPool pool;
        
        [SerializeField] int elementsToAddCount = 4;
        
        private IReadOnlyList<string> _history;

        private readonly Dictionary<int, TextMeshProUGUI> _currentElements = new();

        private void Start()
        {
            scroll.onValueChanged.AddListener(OnScrollValueChanged);
        }
        
        public void SetHistory(IReadOnlyList<string> history)
        {
            _history = history;
            SetResultsHeight();
        }
        
        public void SetResultsHeight()
        {
            CalcResultsHeight();

            SetHistoryHeight();
            
            WaitAndSetScrollToEnd();
        }

        private void CalcResultsHeight()
        {
            float height = Mathf.Min(_history.Count, maxResultElements) * (pool.ElementHeight + elementSpacing) ;
            results.sizeDelta = new Vector2(results.sizeDelta.x, height);
        }

        private void SetHistoryHeight()
        {
            float height = _history.Count == 0
                ? 0
                : _history.Count * pool.ElementHeight + (_history.Count - 1) * elementSpacing;
            historyParent.sizeDelta = new Vector2(historyParent.sizeDelta.x, height);
        }

        private void WaitAndSetScrollToEnd()
        {
            IEnumerator WaitOneFrameAnSetScrollToEnd()
            {
                yield return null;
                scroll.normalizedPosition = Vector2.zero;
            }

            StartCoroutine(WaitOneFrameAnSetScrollToEnd());
        }

        private void OnScrollValueChanged(Vector2 value)
        {
            int index = CalcElementIndex(1 - value.y);

            RemoveElements(index);
            
            for(int i = -elementsToAddCount; i <= elementsToAddCount; i++) 
                AddElementToIndex(index + i);
        }

        private int CalcElementIndex(float pos) => 
            Math.Clamp((int)(pos * _history.Count), 0, _history.Count - 1);

        private void RemoveElements(int index)
        {
            List<int> keysToRemove = new List<int>();
            foreach (KeyValuePair<int, TextMeshProUGUI> element in _currentElements)
            {
                if (element.Key < index - elementsToAddCount || element.Key > index + elementsToAddCount)
                {
                    keysToRemove.Add(element.Key);
                }
            }

            foreach (int key in keysToRemove)
            {
                TextMeshProUGUI element = _currentElements[key];
                pool.ReturnObject(element);
                _currentElements.Remove(key);
            }
        }

        private void AddElementToIndex(int index)
        {
            if (_history.Count > 0 && !_currentElements.ContainsKey(index) && index >= 0 && index < _history.Count)
            {
                TextMeshProUGUI element = pool.GetObject();
                SetupElement(index, element);
                _currentElements.Add(index, element);
            }
        }

        private void SetupElement(int index, TextMeshProUGUI element)
        {
            element.transform.SetParent(historyParent);
            element.rectTransform.anchoredPosition = GetPositionFromIndex(index);
            element.text = _history[index];
        }

        private Vector3 GetPositionFromIndex(int index)
        {
            float yPos = index == 0
                ? 0
                : -index * pool.ElementHeight - (index - 1) * elementSpacing;
            return new Vector3(-10, yPos);
        }
    }
}