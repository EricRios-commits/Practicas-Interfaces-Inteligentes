using TMPro;
using UnityEngine;

namespace P4
{
    public class PointDisplayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI pointsText;
        
        private void OnEnable()
        {
            PointManager.PointsUpdatedEvent += UpdatePointsDisplay;
        }
        
        private void OnDisable()
        {
            PointManager.PointsUpdatedEvent -= UpdatePointsDisplay;
        }
        
        private void UpdatePointsDisplay(int newPoints)
        {
            if (pointsText != null)
            {
                pointsText.text = "Points: " + newPoints;
            }
        }
    }
}