using System.Linq;
using UnityEngine;

namespace P5
{
    public class RecoverObject : MonoBehaviour
    {
        private TreasureObject[] _treasureObjects;
        
        private void Start()
        {
            _treasureObjects = GameObject.FindGameObjectsWithTag("Treasure")
                .Select(go => go.GetComponent<TreasureObject>())
                .ToArray();
        }
        
        public void OnPointerEnter()
        {
            MoveAllObjectsToPlayer();
        }
        
        public void OnPointerExit()
        {
            StopAllObjectsMovement();
        }

        private void MoveAllObjectsToPlayer()
        {
            foreach (var treasure in _treasureObjects)
            {
                if (treasure.IsCollected) continue;
                treasure.MoveTowardsPlayer();
            }
        }
        
        private void StopAllObjectsMovement()
        {
            foreach (var treasure in _treasureObjects)
            {
                treasure.StopMovement();
            }
        }
    }
}