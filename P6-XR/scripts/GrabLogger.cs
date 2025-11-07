using UnityEngine;

namespace Ses1
{
    public class GrabLogger : MonoBehaviour
    {
        public void OnGrab()
        {
            Debug.Log("Object grabbed: " + gameObject.name);
        }
    }
}