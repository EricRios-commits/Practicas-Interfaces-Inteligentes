using UnityEngine;

namespace PMicro
{
    public class WebCamDisplay : MonoBehaviour
    {
        [SerializeField] private Renderer targetRenderer;
        [SerializeField] private string snapShotRoot = "WebCamSnapshots";
        [SerializeField] private KeyCode snapShotKey = KeyCode.P;
        
        private WebCamTexture webCamTexture;
        private string webCamName;

        private void Start()
        {
            if (WebCamTexture.devices.Length > 0)
            {
                webCamTexture = new WebCamTexture(WebCamTexture.devices[0].name, 1920, 1080, 30);
                webCamName = WebCamTexture.devices[0].name;
                Debug.Log("Using webcam: " + webCamName);
                if (targetRenderer != null)
                {
                    targetRenderer.material.mainTexture = webCamTexture;
                }
                
                webCamTexture.Play();
            }
            else
            {
                Debug.LogWarning("No webcam devices found.");
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(snapShotKey))
            {
                TakeAndSaveSnapshot();
            }
        }
        
        private void TakeAndSaveSnapshot()
        {
            if (webCamTexture == null) return;

            Texture2D snapShot = new Texture2D(webCamTexture.width, webCamTexture.height);
            snapShot.SetPixels(webCamTexture.GetPixels());
            snapShot.Apply();

            byte[] bytes = snapShot.EncodeToPNG();
            string directoryPath = System.IO.Path.Combine(Application.persistentDataPath, snapShotRoot);
            if (!System.IO.Directory.Exists(directoryPath))
            {
                System.IO.Directory.CreateDirectory(directoryPath);
            }
            string filePath = System.IO.Path.Combine(directoryPath, $"WebCamSnapshot_{webCamName}_{System.DateTime.Now:yyyyMMdd_HHmmss}.png");
            System.IO.File.WriteAllBytes(filePath, bytes);
            Debug.Log("Webcam snapshot saved to: " + filePath);

            Destroy(snapShot);
        }

        private void OnDisable()
        {
            if (webCamTexture != null)
            {
                webCamTexture.Stop();
            }
        }

        private void OnDestroy()
        {
            if (webCamTexture != null)
            {
                Destroy(webCamTexture);
            }
        }
    }
}