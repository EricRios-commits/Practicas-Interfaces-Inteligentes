using UnityEngine;

namespace P4
{
    public class ShieldAligner : MonoBehaviour
    {
        [SerializeField] private float distanceToAlign;
        [SerializeField] private PhysicalShield[] objectsToAlign;
        [SerializeField] private int pointsThreshold = 200;

        private Vector3 _line;

        private void OnEnable()
        {
            _line = -transform.forward;
            PointManager.PointsUpdatedEvent += HandleAlign;
        }

        private void HandleAlign(int points)
        {
            if (points >= pointsThreshold)
            {
                AlignObjects();
            }
        }
        
        private void AlignObjects()
        {
            for (int i = 0; i < objectsToAlign.Length; i++)
            {
                objectsToAlign[i].MoveTo(transform.position + _line * (i + 1) * distanceToAlign);
            }
        }
    }
}