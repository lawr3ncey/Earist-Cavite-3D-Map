using System;
using System.Net;
using System.Net.Mail;
using UnityEngine;

public class EmailSender : MonoBehaviour
{
    // Gmail credentials (use App Password for security)
    private string senderEmail = "babelonia.l.bscs@gmail.com";
    private string senderPassword = "ckqolloeymryxobn"; // Use an App Password here

    // Method to send OTP email
    public void SendOtpEmail(string recipientEmail, string otp)
    {
        try
        {
            // Create the email message
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(senderEmail);
            mail.To.Add(recipientEmail);
            mail.Subject = "Your OTP for Admin Password Change";
            mail.Body = $"Hello,\n\nYour OTP is: {otp}\n\nPlease use this to proceed with your request.";

            // Configure the SMTP client
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
            smtp.EnableSsl = true;

            // Send the email
            smtp.Send(mail);
            Debug.Log("OTP email sent successfully!");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to send email: {ex.Message}");
        }
    }

    // Helper method to generate a random OTP
    public string GenerateOtp(int length = 6)
    {
        string characters = "0123456789";
        System.Random random = new System.Random();
        char[] otp = new char[length];

        for (int i = 0; i < length; i++)
        {
            otp[i] = characters[random.Next(characters.Length)];
        }

        return new string(otp);
    }
}
