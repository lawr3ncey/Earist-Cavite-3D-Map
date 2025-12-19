using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OTPManager : MonoBehaviour
{
    private string generatedOTP;
    private string userEmail;

    public GameObject passwordChangePanel; // Panel for changing password
    public InputField emailInput; // Input for Gmail address
    public InputField otpInput;   // Input for OTP entered by the user
    public InputField newPasswordInput; // New password input
    public Text feedbackText;     // To display feedback messages

    // Function to generate OTP
    public void GenerateOTP()
    {
        // Simulating OTP generation (this should be a random number or alphanumeric string)
        generatedOTP = Random.Range(100000, 999999).ToString();  // 6-digit OTP

        userEmail = emailInput.text;

        if (IsValidEmail(userEmail))
        {
            // Simulate sending OTP to the user's email
            SendOTPToEmail(userEmail, generatedOTP);
        }
        else
        {
            feedbackText.text = "Invalid Gmail address.";
        }
    }

    private bool IsValidEmail(string email)
    {
        return email.Contains("@") && email.Contains(".");
    }

    // Simulated method to send OTP (you would integrate with an email service here)
    private void SendOTPToEmail(string email, string otp)
    {
        // In a real-world scenario, use an API like SendGrid or SMTP to send the email.
        Debug.Log($"Sending OTP {otp} to {email}");

        // For now, just display the OTP for testing
        feedbackText.text = $"OTP sent to {email}. Please check your inbox.";
    }

    // Function to verify the OTP
    public void VerifyOTPAndChangePassword()
    {
        if (otpInput.text == generatedOTP)
        {
            string newPassword = newPasswordInput.text;

            // Update password (you'd store it in a secure database or server)
            Debug.Log("Password changed successfully.");
            feedbackText.text = "Password changed successfully.";

            passwordChangePanel.SetActive(false);  // Hide password change panel
        }
        else
        {
            feedbackText.text = "Invalid OTP.";
        }
    }
}
