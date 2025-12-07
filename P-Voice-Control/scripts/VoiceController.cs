using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using Whisper;
using Whisper.Utils;

namespace PVoz
{
    public class VoiceController : MonoBehaviour
    {
        [Header("Whisper Components")]
        [SerializeField] private WhisperManager whisperManager;
        [SerializeField] private MicrophoneRecord microphoneRecord;

        [Header("Voice Controlled Objects")]
        [SerializeField] private GameObject[] voiceControlledObjects;
        [SerializeField] private int selectedObjectIndex = 0;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI debugText;

        [Header("Input Settings")]
        [SerializeField] private KeyCode recordingKey = KeyCode.R;

        private IVoiceControlled[] _voiceControlledComponents;
        private WhisperStream _stream;
        private bool _isInitialized;
        private bool _isRecording;

        private Dictionary<string, System.Action<IVoiceControlled>> _commandPatterns;
        public Command[] commands;

        private void Awake()
        {
            SetupVoiceControlledComponents();
            SetupCommandPatterns();
        }

        private void SetupCommandPatterns()
        {
            _commandPatterns = new Dictionary<string, System.Action<IVoiceControlled>>();
            foreach (var command in commands)
            {
                if (!_commandPatterns.ContainsKey(command.commandText))
                {
                    _commandPatterns.Add(command.commandText, voiceControlled =>
                    {
                        switch (command.commandType)
                        {
                            case Command.CommandType.MoveForward:
                                voiceControlled.MoveForward();
                                break;
                            case Command.CommandType.MoveBackward:
                                voiceControlled.MoveBackward();
                                break;
                            case Command.CommandType.StopMovement:
                                voiceControlled.StopMovement();
                                break;
                            case Command.CommandType.TurnLeft:
                                voiceControlled.TurnLeft();
                                break;
                            case Command.CommandType.TurnRight:
                                voiceControlled.TurnRight();
                                break;
                        }
                    });
                }
                else
                {
                    LogError($"Duplicate command pattern detected: {command.commandText}");
                }
            }
        }

        private async void Start()
        {
            if (whisperManager == null)
            {
                LogError("WhisperManager is not assigned!");
                return;
            }
            if (microphoneRecord == null)
            {
                LogError("MicrophoneRecord is not assigned!");
                return;
            }
            try
            {
                while (!whisperManager.IsLoaded)
                {
                    await System.Threading.Tasks.Task.Delay(100);
                }
                _stream = await whisperManager.CreateStream(microphoneRecord);
                if (_stream == null)
                {
                    LogError("Failed to create Whisper stream!");
                    return;
                }
                _stream.OnSegmentFinished += OnSegmentFinished;
                _stream.OnStreamFinished += OnStreamFinished;
                microphoneRecord.OnRecordStop += OnRecordStop;
                _isInitialized = true;
                Debug.Log("Voice controller initialized successfully!");
            }
            catch (System.Exception ex)
            {
                LogError($"Error initializing voice controller: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void Update()
        {
            if (!_isInitialized)
                return;
            // if (Input.GetKeyDown(recordingKey))
            // {
            //     ToggleRecording();
            // }
        }

        public void ToggleRecording()
        {
            if (_isRecording)
            {
                StopRecording();
            }
            else
            {
                StartRecording();
            }
        }

        private void StartRecording()
        {
            if (_stream == null || microphoneRecord == null)
            {
                LogError("Cannot start recording - components not initialized!");
                return;
            }
            _stream.StartStream();
            microphoneRecord.StartRecord();
            _isRecording = true;
            UpdateDebugText($"Recording... Press '{recordingKey}' to stop");
            Debug.Log("Started recording");
        }

        private void StopRecording()
        {
            if (microphoneRecord == null)
                return;
            microphoneRecord.StopRecord();
            _isRecording = false;
            UpdateDebugText($"Recording stopped. Press '{recordingKey}' to start again");
            Debug.Log("Stopped recording");
        }

        private void OnDestroy()
        {
            if (_stream != null)
            {
                _stream.OnSegmentFinished -= OnSegmentFinished;
                _stream.OnStreamFinished -= OnStreamFinished;
            }
            if (microphoneRecord != null)
            {
                microphoneRecord.OnRecordStop -= OnRecordStop;
            }
        }

        private void OnSegmentFinished(WhisperResult segment)
        {
            if (!_isInitialized || segment == null)
                return;
            var command = segment.Result.ToLower().Trim();

            if (string.IsNullOrEmpty(command))
                return;
            UpdateDebugText($"Heard: {command}");
            Debug.Log($"Command received: {command}");
            ProcessCommand(command);
        }

        private void ProcessCommand(string input)
        {
            if (selectedObjectIndex < 0 || selectedObjectIndex >= _voiceControlledComponents.Length)
            {
                LogError($"Invalid selected object index: {selectedObjectIndex}");
                return;
            }
            var voiceControlled = _voiceControlledComponents[selectedObjectIndex];
            if (voiceControlled == null)
            {
                LogError($"Voice controlled component at index {selectedObjectIndex} is null!");
                return;
            }
            var matchedCommands = new List<(string pattern, System.Action<IVoiceControlled> action, Match match)>();
            foreach (var kvp in _commandPatterns)
            {
                var match = Regex.Match(input, kvp.Key, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    matchedCommands.Add((kvp.Key, kvp.Value, match));
                }
            }
            if (matchedCommands.Count == 0)
            {
                UpdateDebugText($"Unknown command: {input}");
                Debug.Log($"No matching command found for: {input}");
                return;
            }
            var firstMatch = matchedCommands.OrderBy(c => c.match.Index).First();
            firstMatch.action(voiceControlled);
            var commandName = GetCommandName(firstMatch.pattern);
            UpdateDebugText($"{commandName}");
            Debug.Log($"Executed command: {commandName} (matched '{firstMatch.match.Value}')");
        }

        private string GetCommandName(string pattern)
        {
            var match = Regex.Match(pattern, @"\((.*?)\|");
            if (match.Success)
            {
                return match.Groups[1].Value.Replace("\\b", "").Trim();
            }
            return "Command";
        }

        private void OnStreamFinished(string finalResult)
        {
            Debug.Log($"Stream finished with result: {finalResult}");
        }

        private void OnRecordStop(AudioChunk recordedAudio)
        {
            Debug.Log("Recording stopped");
        }

        private void SetupVoiceControlledComponents()
        {
            if (voiceControlledObjects == null || voiceControlledObjects.Length == 0)
            {
                LogError("No voice controlled objects assigned!");
                _voiceControlledComponents = new IVoiceControlled[0];
                return;
            }
            _voiceControlledComponents = new IVoiceControlled[voiceControlledObjects.Length];
            for (int i = 0; i < voiceControlledObjects.Length; i++)
            {
                if (voiceControlledObjects[i] == null)
                {
                    LogError($"Voice controlled object at index {i} is null!");
                    continue;
                }
                var component = voiceControlledObjects[i].GetComponent<IVoiceControlled>();
                if (component == null)
                {
                    LogError($"GameObject '{voiceControlledObjects[i].name}' doesn't have an IVoiceControlled component!");
                    continue;
                }
                _voiceControlledComponents[i] = component;
            }
            Debug.Log($"Setup {_voiceControlledComponents.Length} voice controlled components.");
        }

        private void UpdateDebugText(string message)
        {
            if (debugText != null)
            {
                debugText.text = message;
            }
        }

        private void LogError(string message)
        {
            Debug.LogError($"[VoiceController] {message}");
            UpdateDebugText($"ERROR: {message}");
        }
    }
}
