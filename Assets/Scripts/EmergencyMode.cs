using UnityEngine;

public class EmergencyMode : MonoBehaviour
{
    public PathRenderer pathRenderer; // Assign PathRenderer in Inspector
    private bool isEmergencyMode = false;
    public VoiceAssistantOutput voiceAssistant;

    public void ActivateEmergencyMode()
    {
        isEmergencyMode = !isEmergencyMode; // Toggle the emergency mode
        pathRenderer.SetEmergencyMode(isEmergencyMode);

        if (voiceAssistant != null)
        {
            if (isEmergencyMode)
            {
                voiceAssistant.Speak("Emergency Mode Activated. Follow the path to the nearest emergency exit.");
            }
            else
            {
                voiceAssistant.Speak("Emergency Mode Deactivated.");
            }
        }
        else
        {
            Debug.LogError("VoiceAssistant not found in the scene.");
        }


        Debug.Log($"Emergency Mode: {(isEmergencyMode ? "Activated" : "Deactivated")}");
    }
}
