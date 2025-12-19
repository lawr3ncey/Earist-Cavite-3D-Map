using UnityEngine;
using UnityEngine.UI;

public class RoleSwitcher : MonoBehaviour
{
    // Reference to the admin login panel
    public GameObject adminLoginPanel;
    public Text feedbackText;
    public Text modeText;               // Reference to display current mode ("ADMIN MODE" or "USER MODE")
    public GameObject userModeUI;
    public GameObject warningPanel;
    private bool isInAdminMode = false;
    public GameObject adminUIElements;
    public InputField usernameInput;
    public InputField passwordInput;
    private string adminUsername = "admin";
    private string adminPassword = "password";

    public VoiceAssistantOutput voiceAssistant;


    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        InitializeMode(); // Ensure the correct mode is displayed at the start
    }

    // Initialize the mode on game start
    private void InitializeMode()
    {
        modeText.text = "USER MODE"; // Default to User Mode
        adminUIElements.SetActive(false); // Hide Admin UI
        userModeUI.SetActive(true); // Show User Mode UI
        isInAdminMode = false; // Ensure we are in User Mode by default
    }

    // Show the Admin Login Panel
    public void ShowAdminLoginPanel()
    {
        adminLoginPanel.SetActive(true);

        // Trigger voice output to say "Welcome Admin!"
        if (voiceAssistant != null)
        {
            voiceAssistant.Speak("Welcome Admin!");
        }
        else
        {
            Debug.LogError("VoiceAssistant not found in the scene.");
        }
    }

    // Handle Login Button Click
    public void OnLoginButtonClicked()
    {
        feedbackText.text = ""; // Clear any previous feedback

        if (usernameInput.text == adminUsername && passwordInput.text == adminPassword)
        {
            adminLoginPanel.SetActive(false); // Hide the login panel
            adminUIElements.SetActive(true);  // Show the Admin UI
            isInAdminMode = true;             // Set to Admin Mode

            modeText.text = "ADMIN MODE";     // Update the mode text to show "ADMIN MODE"

            if (voiceAssistant != null)
            {
                voiceAssistant.Speak("Admin mode activated.");
            }
            else
            {
                Debug.LogError("VoiceAssistant not found in the scene.");
            }


            Debug.Log("Admin mode activated.");
        }
        else
        {
            feedbackText.text = "Invalid credentials. Try User Mode.";
            feedbackText.gameObject.SetActive(true); // Show feedback if credentials are incorrect

            if (voiceAssistant != null)
            {
                voiceAssistant.Speak("Invalid credentials. Please try User Mode.");
            }
            else
            {
                Debug.LogError("VoiceAssistant not found in the scene.");
            }

            Debug.Log("Invalid credentials. Please try again.");
        }

        // Clear input fields after login attempt
        usernameInput.text = "";
        passwordInput.text = "";
    }

    // Handle Cancel Button Click
    public void OnCancelButtonClicked()
    {
        adminLoginPanel.SetActive(false); // Hide Login Panel
    }


    // Method to try switching to User Mode (with a warning)
    public void TrySwitchToUserMode()
    {
        // Show warning if switching back from Admin Mode
        if (isInAdminMode)
        {
            warningPanel.SetActive(true); // Show warning panel with Yes/No buttons
        }
        else
        {
            // If not in Admin Mode, switch directly to User Mode
            modeText.text = "USER MODE"; // Update mode text to "USER MODE"
            adminUIElements.SetActive(false); // Hide Admin UI
            userModeUI.SetActive(true); // Show User Mode UI
            isInAdminMode = false; // Mark as User Mode
        }
    }




    // Method to confirm switching to User Mode (Yes button)
    public void SwitchToUserMode()
    {
        modeText.text = "USER MODE"; // Update mode text to "USER MODE"
        adminUIElements.SetActive(false); // Hide Admin UI
        userModeUI.SetActive(true); // Show User Mode UI
        isInAdminMode = false; // Mark as User Mode
        warningPanel.SetActive(false); // Hide the warning panel
    }

    // Method to cancel switching to User Mode (No button)
    public void CancelSwitchToUserMode()
    {
        warningPanel.SetActive(false);     // Hide the warning panel
    }
}