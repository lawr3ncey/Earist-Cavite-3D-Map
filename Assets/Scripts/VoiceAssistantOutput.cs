using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Collections;

public class VoiceAssistantOutput : MonoBehaviour
{
    public AudioSource audioSource;

    private string apiKey = "AIzaSyA4qAxunk2PyfsEpWOdiEmaZKbgV-F3i0A";

    // Method to trigger text-to-speech
    public void Speak(string message)
    {
        StartCoroutine(GetAudioFromGoogleTTS(message));
    }

    private IEnumerator GetAudioFromGoogleTTS(string text)
    {
        string url = "https://texttospeech.googleapis.com/v1/text:synthesize?key=" + apiKey;

        var requestBody = new
        {
            input = new { text = text },
            voice = new { languageCode = "en-US", name = "en-US-Standard-B", ssmlGender = "NEUTRAL" },
            audioConfig = new { audioEncoding = "LINEAR16" }
        };

        string jsonBody = JsonConvert.SerializeObject(requestBody);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            var response = JsonConvert.DeserializeObject<GoogleTTSResponse>(responseText);
            if (string.IsNullOrEmpty(response.audioContent))
            {
                Debug.LogError("audioContent is empty or null.");
            }
            else
            {
                byte[] audioBytes = System.Convert.FromBase64String(response.audioContent);
                string filePath = Path.Combine(Application.persistentDataPath, "TTSOutput.wav");
                File.WriteAllBytes(filePath, audioBytes);
                StartCoroutine(LoadWavFile(filePath));
            }
        }
        else
        {
            Debug.LogError("TTS Error: " + request.error);
        }
    }

    private IEnumerator LoadWavFile(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
        AudioClip audioClip = WavUtility.ToAudioClip(fileData, 0, "TTS Audio");

        if (audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
            Debug.Log("Audio played successfully.");
        }
        else
        {
            Debug.LogError("Failed to load AudioClip from WAV file.");
        }

        yield return null;
    }

    [System.Serializable]
    private class GoogleTTSResponse
    {
        public string audioContent;
    }
}
