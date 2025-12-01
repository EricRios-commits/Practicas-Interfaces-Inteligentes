using TMPro;
using UnityEngine;

namespace PSensores
{
    public class SensorController : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private float inputSpeedMultiplier = 5f;
        [SerializeField] private Vector2 acceptedLatitudeRange = new Vector2(-90f, 90f);
        [SerializeField] private Vector2 acceptedLongitudeRange = new Vector2(-180f, 180f);
        [SerializeField] private TextMeshProUGUI text;

        private LocationService _gps;
        private Rigidbody _rigidbody;

        private float _cachedHeading;
        private Vector3 _cachedAcceleration;

        private void Start()
        {
            Input.gyro.enabled = true;
            Input.compass.enabled = true;

            _gps = Input.location;
            _gps.Start(1f, 0.1f);

            _rigidbody = GetComponent<Rigidbody>();

            if (_rigidbody == null)
            {
                Debug.LogError("No Rigidbody component found. Please attach a Rigidbody to this GameObject.");
            }
            else
            {
                _rigidbody.isKinematic = true;
            }
        }

        private void Update()
        {
            CacheCompass();
            CacheAccelerometer();
        }

        private void FixedUpdate()
        {
            EnsureLookingNorth();
            MoveWithAccelerometer();
            DumpDebugToText();
        }


        private void CacheCompass()
        {
            if (Input.compass.enabled)
            {
                _cachedHeading = Input.compass.trueHeading;
            }
        }

        private void CacheAccelerometer()
        {
            _cachedAcceleration = Input.acceleration;
        }


        private void MoveWithAccelerometer()
        {
            if (_rigidbody == null) return;
            if (!IsInLatitudeAndLongitudeRange()) return;

            float forwardAmount = -_cachedAcceleration.z;
            Vector3 velocity = transform.forward * (forwardAmount * inputSpeedMultiplier);
            transform.position += velocity * Time.fixedDeltaTime;
        }

        private bool IsInLatitudeAndLongitudeRange()
        {
            if (_gps == null || _gps.status != LocationServiceStatus.Running) return true;

            float latitude = _gps.lastData.latitude;
            float longitude = _gps.lastData.longitude;

            return latitude >= acceptedLatitudeRange.x && latitude <= acceptedLatitudeRange.y &&
                   longitude >= acceptedLongitudeRange.x && longitude <= acceptedLongitudeRange.y;
        }


        private void EnsureLookingNorth()
        {
            float heading = _cachedHeading;
            Quaternion target = Quaternion.Euler(0, -heading + 90f, 0);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                target,
                rotationSpeed * Time.fixedDeltaTime
            );
        }


        private void DumpDebugToText()
        {
            if (text == null) return;

            Vector3 acc = _cachedAcceleration;
            float forwardSpeed = -acc.z;
            Vector3 velocity = transform.forward * (forwardSpeed * inputSpeedMultiplier);
            Vector3 delta = velocity * Time.fixedDeltaTime;

            string gpsStatus = _gps != null ? _gps.status.ToString() : "null";
            string latLong = "n/a";
            if (_gps != null && _gps.status == LocationServiceStatus.Running)
            {
                latLong = $"lat:{_gps.lastData.latitude:F6}, lon:{_gps.lastData.longitude:F6}";
            }

            string rbInfo = _rigidbody != null
                ? $"pos:{_rigidbody.position:F3} isKinematic:{_rigidbody.isKinematic}"
                : "Rigidbody:null";

            Quaternion targetQuat = Quaternion.Euler(0, -_cachedHeading + 90f, 0);

            string currentEuler = transform.rotation.eulerAngles.ToString("F3");
            string targetEuler = targetQuat.eulerAngles.ToString("F3");

            string debug = $"Accelerometer (Input.acceleration): yes\n" +
                           $"Raw accel: {Vec3(acc)}\n" +
                           $"Forward speed (corrected Z): {forwardSpeed:F3}\n" +
                           $"Velocity: {Vec3(velocity)}\n" +
                           $"Delta this step: {Vec3(delta)}\n\n" +
                           $"Compass: yes\n" +
                           $"Heading (true north): {_cachedHeading:F2}\n" +
                           $"Target euler: {targetEuler}\n" +
                           $"Current euler: {currentEuler}\n\n" +
                           $"GPS status: {gpsStatus}\n" +
                           $"Location: {latLong}\n\n" +
                           $"Rigidbody: {rbInfo}";

            text.text = debug;
        }

        private static string Vec3(Vector3 v)
        {
            return $"({v.x:F3}, {v.y:F3}, {v.z:F3})";
        }
    }
}