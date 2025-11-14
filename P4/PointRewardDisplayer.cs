using UnityEngine;

namespace P4
{
    public class PointRewardDisplayer : MonoBehaviour
    {
        [SerializeField] private GameObject reward;
        [SerializeField] private int pointsThreshold = 100;
        
        private void OnEnable()
        {
            PointManager.PointsUpdatedEvent += UpdatePointsDisplay;
            reward.SetActive(false);
        }
        
        private void OnDisable()
        {
            PointManager.PointsUpdatedEvent -= UpdatePointsDisplay;
        }
        
        private void UpdatePointsDisplay(int newPoints)
        {
            if (newPoints >= pointsThreshold)
            {
                reward.SetActive(true);
            }
        }
    }
}