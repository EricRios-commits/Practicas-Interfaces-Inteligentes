using UnityEngine;

namespace P4
{
    public class PointManager : MonoBehaviour
    {
        public delegate void OnPointsUpdated(int newPoints);
        public static event OnPointsUpdated PointsUpdatedEvent;
        
        private static int points = 0;
        
        public static void AddPoints(int amount)
        {
            points += amount;
            Debug.Log("Points: " + points);
            PointsUpdatedEvent?.Invoke(points);
        }
    }
}