using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using Newtonsoft.Json.Linq;

public class SpeechToText : MonoBehaviour
{
    public static SpeechToText Instance { get; private set; }

    private string apiKey = "AIzaSyA4qAxunk2PyfsEpWOdiEmaZKbgV-F3i0A"; // Make sure to secure this if in production

    public GameObject firstPersonController; // Assign this in the Unity Inspector
    public NLPManager nlpManager; // Reference to the NLPManager

    public VoiceAssistantOutput voiceAssistant;

    private List<string> roomNames = new List<string>
    {
        "Criminology Building", "Canteen", "Covered Court", "Stage", "Basketball Court",
        "Administration Office", "Registrars Office", "Speech Laboratory", "Internet Room",
        "Library Undergrad Program", "Graduate Office", "Room 302", "Computer Laboratory",
        "MPH Room 1", "MPH Room 2", "MPH Room 3", "Pent House", "Faculty Room",
        "Physics Laboratory", "Psychology Laboratory", "Chemistry Laboratory",
        "Library Graduate Program", "Room 203", "Head Quarters", "Room 101", "Room 102",
        "Room 103", "Room 104", "Room 105", "Room 106", "Room 107", "Room 204",
        "Room 205", "Room 206", "Room 207", "Room 208", "Room 209", "Room 210",
        "Room 211", "Comfort Room", "Annex 101", "Annex Building", "Annex 201", "Annex 301", "Annex 102",
        "Annex 202", "Annex 302", "Annex 103", "Annex 203", "Annex 303", "Conference Room",
        "School Clinic", "Dragon Fruit Farm", "Evacuation Spot A", "Evacuation Spot B", "Fire Exit",
        "Fire Exit", "Fire Exit 1", "Fire Exit 2", "Fire Exit 3", "Fire Exit 4", "Fire Exit 5",

        "Main Building", "Entrance"
    };

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SendAudioToGoogle(AudioClip clip)
    {
        StartCoroutine(SendAudioClip(clip));
    }

    private IEnumerator SendAudioClip(AudioClip clip)
    {
        byte[] audioData = ConvertAudioClipToWAV(clip);
        if (audioData == null)
        {
            Debug.LogError("Failed to convert AudioClip to WAV format.");
            yield break;
        }

        string jsonData = JsonifyAudioRequest(audioData);

        using (UnityWebRequest request = new UnityWebRequest($"https://speech.googleapis.com/v1/speech:recognize?key={apiKey}", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string resultText = request.downloadHandler.text;
                Debug.Log("Transcription Result: " + resultText);
                ProcessVoiceCommand(resultText);
            }
            else
            {
                Debug.LogError("Error in Speech-to-Text request: " + request.error);
                Debug.LogError("Error response: " + request.downloadHandler.text);
            }
        }
    }

    private void ProcessVoiceCommand(string response)
    {
        Debug.Log("Received response from Speech-to-Text API: " + response);

        string command = ExtractCommandFromResponse(response);
        Debug.Log("Extracted command: " + command);

        // Check if the command matches any room name
        foreach (string room in roomNames)
        {
            if (command.Contains(room, StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log($"Command matched '{room}'. Navigating to {room}.");

                // Trigger voice output to speak the result
                if (voiceAssistant != null)
                {
                    voiceAssistant.Speak($"Command matched '{room}'. Navigating to {room}.");
                }
                else
                {
                    Debug.LogError("VoiceAssistantOutput is not found in the scene.");
                }

                NavigateTo(room); // Call your navigation logic
                return; // Exit once a match is found
            }
        }

        // If no match is found
        Debug.LogWarning("No recognized location in command. Command text: " + command);
        if (voiceAssistant != null)
        {
            voiceAssistant.Speak("Sorry, I couldn't find that room. Please try again.");
        }
    }


    private string ExtractCommandFromResponse(string response)
    {
        try
        {
            var jsonResponse = JObject.Parse(response);
            var transcript = jsonResponse["results"]?[0]?["alternatives"]?[0]?["transcript"]?.ToString();
            return transcript ?? string.Empty;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error parsing response JSON: " + ex.Message);
            return string.Empty;
        }
    }

    private void NavigateTo(string location)
    {
        Vector3 targetPosition = LocationManager.Instance.GetLocationPosition(location);

        if (firstPersonController == null)
        {
            Debug.LogError("First-person controller is not assigned.");
            return;
        }

        Debug.Log($"Navigating from {firstPersonController.transform.position} to {targetPosition}");

        if (targetPosition != Vector3.zero)
        {
            PathRenderer.Instance.DrawPath(firstPersonController.transform.position, targetPosition, true);
        }
        else
        {
            Debug.LogWarning("Target position for the location could not be found.");
        }
    }

    private byte[] ConvertAudioClipToWAV(AudioClip clip)
    {
        try
        {
            if (clip.samples == 0)
            {
                Debug.LogError("AudioClip contains no audio data.");
                return null;
            }

            float[] samples = new float[clip.samples];
            clip.GetData(samples, 0);

            Int16[] intData = new Int16[samples.Length];
            Byte[] bytesData = new Byte[samples.Length * sizeof(Int16)];
            const float rescaleFactor = 32767; // Convert to 16-bit PCM

            for (int i = 0; i < samples.Length; i++)
            {
                intData[i] = (short)(samples[i] * rescaleFactor);
                Byte[] byteArr = BitConverter.GetBytes(intData[i]);
                byteArr.CopyTo(bytesData, i * sizeof(Int16));
            }

            byte[] header = GetWAVHeader(clip, bytesData.Length);
            byte[] wavFile = new byte[header.Length + bytesData.Length];

            Buffer.BlockCopy(header, 0, wavFile, 0, header.Length);
            Buffer.BlockCopy(bytesData, 0, wavFile, header.Length, bytesData.Length);

            return wavFile;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error during WAV conversion: " + ex.Message);
            return null;
        }
    }

    private string JsonifyAudioRequest(byte[] audioData)
    {
        string base64Audio = Convert.ToBase64String(audioData);
        return $@"
        {{
            ""config"": {{
                ""encoding"": ""LINEAR16"",
                ""sampleRateHertz"": 16000,
                ""languageCode"": ""en-US""
            }},
            ""audio"": {{
                ""content"": ""{base64Audio}""
            }}
        }}";
    }

    private byte[] GetWAVHeader(AudioClip clip, int dataLength)
    {
        int sampleRate = clip.frequency;
        int channels = clip.channels;
        int byteRate = sampleRate * channels * 2; // 16-bit PCM

        byte[] header = new byte[44];

        // RIFF header
        Buffer.BlockCopy(Encoding.ASCII.GetBytes("RIFF"), 0, header, 0, 4);
        Buffer.BlockCopy(BitConverter.GetBytes(36 + dataLength), 0, header, 4, 4);
        Buffer.BlockCopy(Encoding.ASCII.GetBytes("WAVE"), 0, header, 8, 4);

        // fmt sub-chunk
        Buffer.BlockCopy(Encoding.ASCII.GetBytes("fmt "), 0, header, 12, 4);
        Buffer.BlockCopy(BitConverter.GetBytes(16), 0, header, 16, 4); // Sub-chunk size (16 for PCM)
        Buffer.BlockCopy(BitConverter.GetBytes((short)1), 0, header, 20, 2); // Audio format (1 = PCM)
        Buffer.BlockCopy(BitConverter.GetBytes((short)channels), 0, header, 22, 2);
        Buffer.BlockCopy(BitConverter.GetBytes(sampleRate), 0, header, 24, 4);
        Buffer.BlockCopy(BitConverter.GetBytes(byteRate), 0, header, 28, 4);
        Buffer.BlockCopy(BitConverter.GetBytes((short)(channels * 2)), 0, header, 32, 2); // Block align
        Buffer.BlockCopy(BitConverter.GetBytes((short)16), 0, header, 34, 2); // Bits per sample

        // data sub-chunk
        Buffer.BlockCopy(Encoding.ASCII.GetBytes("data"), 0, header, 36, 4);
        Buffer.BlockCopy(BitConverter.GetBytes(dataLength), 0, header, 40, 4);

        return header;
    }

}
