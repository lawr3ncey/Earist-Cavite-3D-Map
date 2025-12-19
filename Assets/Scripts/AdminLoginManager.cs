using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Required for TextMeshPro UI

public class AdminLoginManager : MonoBehaviour
{
    public TMP_InputField usernameField; // Reference to the username input
    public TMP_InputField passwordField; // Reference to the password input
    public TextMeshProUGUI errorText;    // Reference to the error message display
    public string adminUsername = "admin"; // Replace with your desired admin username
    public string adminPassword = "1234";  // Replace with your desired admin password

    // This method is called when the Login button is clicked
    public void OnLoginButtonClicked()
    {
        // Check if username and password match
        if (usernameField.text == adminUsername && passwordField.text == adminPassword)
        {
            // Load AdminInterfaceScene if credentials are correct
            SceneManager.LoadScene("UserInterfaceScene");
        }
        else
        {
            // Show error message if login fails
            errorText.text = "Invalid username or password. Please try again.";
        }
    }

    // Optional: Method to clear the error text when the user starts typing
    public void ClearErrorText()
    {
        errorText.text = "";
    }
}
