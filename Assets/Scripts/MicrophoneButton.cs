using UnityEngine;
using UnityEngine.UI;

public class MicrophoneButton : MonoBehaviour
{
    public Button microphoneButton;
    private bool isRecording = false;
    private AudioClip recordedClip;

    void Start()
    {
        microphoneButton.onClick.AddListener(OnMicrophoneButtonClicked);
    }

    private void OnMicrophoneButtonClicked()
    {
        if (!isRecording)
        {
            StartRecording();
        }
        else
        {
            StopRecording();
        }
    }

    private void StartRecording()
    {
        isRecording = true;
        recordedClip = Microphone.Start(null, false, 10, 16000); // Assign the AudioClip to the field
        if (recordedClip == null)
        {
            Debug.LogError("Failed to start recording.");
        }
        else
        {
            Debug.Log("Recording started...");
        }
    }

    private void StopRecording()
    {
        isRecording = false;
        // Stop the microphone recording
        Microphone.End(null);

        // Send the recorded audio clip to SpeechToText for transcription
        if (recordedClip != null)
        {
            SpeechToText.Instance.SendAudioToGoogle(recordedClip);
        }
    }



}
