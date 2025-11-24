using UnityEngine;

namespace PMicro
{
    // plays microphone input through audio source when key pressed
    public class MicrophonePlayer : MonoBehaviour
    {
        [SerializeField] private KeyCode playKey = KeyCode.R;

        private AudioSource audioSource;
        private string microphoneDevice;
        private bool isInitialized;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;
            
            if (Microphone.devices.Length > 0)
            {
                microphoneDevice = Microphone.devices[0];
                isInitialized = true;
            }
            else
            {
                Debug.LogWarning("No microphone devices found.");
            }
        }

        private void Update()
        {
            if (!isInitialized) return;

            if (Input.GetKeyDown(playKey))
            {
                if (audioSource.isPlaying)
                {
                    Debug.Log("Stopping microphone playback.");
                    audioSource.Stop();
                    Microphone.End(microphoneDevice);
                }
                else
                {
                    Debug.Log("Starting microphone input.");
                    audioSource.clip = Microphone.Start(microphoneDevice, true, 1, 44100);
                    while (!(Microphone.GetPosition(microphoneDevice) > 0)) { }
                    audioSource.Play();
                }
            }
        }
    }
}