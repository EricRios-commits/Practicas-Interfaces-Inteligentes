using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace PSensores
{
    public class SensorDebugger : MonoBehaviour
    {
        private List<Sensor> _sensors;
        private LocationService _gps;

        public delegate void OnSensorDataChanged(string allSensorData);
        public static event OnSensorDataChanged SensorDataChangedEvent;

        private void Awake()
        {
            _sensors = new List<Sensor>();
        }

        private void OnEnable()
        {
            Debug.Log("OnEnable");
            EnableSensors();
            foreach (var device in InputSystem.devices)
                AddDeviceIfSensor(device);

            InputSystem.onDeviceChange += OnDeviceChange;
        }

        private void OnDisable()
        {
            InputSystem.onDeviceChange -= OnDeviceChange;
            _sensors.Clear();
        }
        
        private void SetupGps()
        {
            Input.location.Start(1f, 0.1f);
            _gps = Input.location;
        }

        private void EnableSensors()
        {
            EnableIfPresent(Accelerometer.current);
            EnableIfPresent(UnityEngine.InputSystem.Gyroscope.current);
            EnableIfPresent(GravitySensor.current);
            EnableIfPresent(AttitudeSensor.current);
            EnableIfPresent(LinearAccelerationSensor.current);
            EnableIfPresent(MagneticFieldSensor.current);
            EnableIfPresent(LightSensor.current);
            EnableIfPresent(PressureSensor.current);
            EnableIfPresent(ProximitySensor.current);
            EnableIfPresent(HumiditySensor.current);
            EnableIfPresent(AmbientTemperatureSensor.current);
            EnableIfPresent(StepCounter.current);
            SetupGps();
        }

        private void EnableIfPresent(InputDevice device)
        {
            if (device != null)
                InputSystem.EnableDevice(device);
        }
        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            switch (change)
            {
                case InputDeviceChange.Added:
                case InputDeviceChange.Reconnected:
                    AddDeviceIfSensor(device);
                    break;
                case InputDeviceChange.Removed:
                case InputDeviceChange.Disconnected:
                    if (device is Sensor removedSensor)
                        _sensors.Remove(removedSensor);
                    break;
            }
        }

        private void AddDeviceIfSensor(InputDevice device)
        {
            if (device is Sensor sensor && !_sensors.Contains(sensor))
                _sensors.Add(sensor);
        }

        private void Update()
        {
            if ((_sensors == null || _sensors.Count == 0) && (_gps == null))
            {
                Debug.Log("No sensors detected.");
                LogToFile("No sensors detected.\n");
                return;
            }

            var sb = new StringBuilder();
            foreach (var sensor in _sensors)
            {
                if (sensor == null) continue;
                sb.AppendLine($"Sensor: {sensor.displayName}");
                foreach (var control in sensor.allControls)
                {
                    if (control is AxisControl axisControl)
                        sb.AppendLine($"  {axisControl.name}: {axisControl.ReadValue()}");
                    if (control is ButtonControl buttonControl)
                        sb.AppendLine($"  {buttonControl.name}: {buttonControl.ReadValue()}");
                    if (control is Vector2Control vector2Control)
                        sb.AppendLine($"  {vector2Control.name}: {vector2Control.ReadValue()}");
                    if (control is Vector3Control vector3Control)
                        sb.AppendLine($"  {vector3Control.name}: {vector3Control.ReadValue()}");
                    if (control is QuaternionControl quaternionControl)
                        sb.AppendLine($"  {quaternionControl.name}: {quaternionControl.ReadValue()}");
                    if (control is IntegerControl integerControl)
                        sb.AppendLine($"  {integerControl.name}: {integerControl.ReadValue()}");
                    if (control is DoubleControl doubleControl)
                        sb.AppendLine($"  {doubleControl.name}: {doubleControl.ReadValue()}");
                }
            }
            if (_gps is { isEnabledByUser: true })
            {
                sb.AppendLine("GPS Data:");
                sb.AppendLine($"  Latitude: {_gps.lastData.latitude}");
                sb.AppendLine($"  Longitude: {_gps.lastData.longitude}");
                sb.AppendLine($"  Altitude: {_gps.lastData.altitude}");
                sb.AppendLine($"  Horizontal Accuracy: {_gps.lastData.horizontalAccuracy}");
                sb.AppendLine($"  Vertical Accuracy: {_gps.lastData.verticalAccuracy}");
                sb.AppendLine($"  Timestamp: {_gps.lastData.timestamp}");
            }
            else
            {
                sb.AppendLine("GPS Data: Not Enabled or Not Available" );
            }
            string sensorData = sb.ToString();
            Debug.Log(sensorData);
            LogToFile(sensorData);
            SensorDataChangedEvent?.Invoke(sensorData);
        }
        
        private void LogToFile(string message)
        {
            string path = System.IO.Path.Combine(Application.persistentDataPath, "SensorLogs.txt");
            System.IO.File.AppendAllText(path, message + "\n");
        }
    }
}