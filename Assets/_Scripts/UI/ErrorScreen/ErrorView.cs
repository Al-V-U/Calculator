using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.ErrorScreen
{
    public class ErrorView : MonoBehaviour
    {
        [SerializeField] private Button button;

        public Action CloseCallback { get; set; }

        private void Start() => 
            button.onClick.AddListener(OnButtonClick);
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
                OnButtonClick();
        }

        private void OnButtonClick()
        {
            CloseCallback?.Invoke();
            Destroy(gameObject);
        }
    }
}
