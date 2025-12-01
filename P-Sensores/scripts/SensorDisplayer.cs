using TMPro;
using UnityEngine;

namespace PSensores
{
    public class SensorDisplayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;

        private void OnEnable()
        {
            SensorDebugger.SensorDataChangedEvent += UpdateDisplay;
        }
        
        private void UpdateDisplay(string message)
        {
            if (textMeshProUGUI != null)
            {
                textMeshProUGUI.text = message;
            }
        }
    }
}