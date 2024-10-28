using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _Scripts.UI.Components
{
    public class ElementPool : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI elementPrefab;
        [SerializeField] private int initialPoolSize = 9;
        [SerializeField] private Transform poolElements;
    
        private readonly List<TextMeshProUGUI> _elementsInPool = new();
        
        public float ElementHeight =>
            elementPrefab.rectTransform.sizeDelta.y;

        private void Awake()
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                TextMeshProUGUI element = Instantiate(elementPrefab, poolElements);
                element.transform.localPosition = Vector3.zero;
                _elementsInPool.Add(element);
            }
        }

        public TextMeshProUGUI GetObject()
        {
            TextMeshProUGUI element;
            if (_elementsInPool.Count == 0)
            {
                element = Instantiate(elementPrefab, poolElements);
                return element;
            }

            element = _elementsInPool[^1];
            _elementsInPool.RemoveAt(_elementsInPool.Count - 1);

            return element;
        }

        public void ReturnObject(TextMeshProUGUI element)
        {
            _elementsInPool.Add(element);
            element.transform.SetParent(poolElements);
            element.transform.localPosition = Vector3.zero;
        }
    }
}
