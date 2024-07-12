using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using UnityEngine;
using TMPro;
using System.IO;


public class EmailData : MonoBehaviour


{
    public GameManager gameManager;

    public string senderEmail;
    public string senderPassword;
    public string defaultRecipientEmail;
  
    public TMP_InputField recipientInput1;
    public TMP_InputField recipientInput2;

    private string recipient1;
    private string recipient2;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }


    public void StoreRecipients()
    {
        recipient1 = recipientInput1.text;
        recipient2 = recipientInput2.text;
    }


    public string DataFromLog()
    {
        if (File.Exists(gameManager.pathToLogs))
        {
            try
            {
                string logData = File.ReadAllText(gameManager.pathToLogs);
                return logData;
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Failed to read log file: " + ex.Message);
                return "Failed to read log data.";
            }
        }
        else
        {
            Debug.LogError("Log file not found at path: " + gameManager.pathToLogs);
            return "Log file not found.";
        }
    }

    public void SendEmails()
    {
        string logData = DataFromLog();

        Send(defaultRecipientEmail, logData);
        Send(recipient1, logData);
        Send(recipient2, logData);
    }


    private void Send(string recipientEmail, string logData)
    {
        if (string.IsNullOrEmpty(recipientEmail))
        {
            Debug.LogWarning("Recipient email is empty, skipping send.");
            return;
        }

        MailMessage mail = new MailMessage();

        mail.From = new MailAddress(senderEmail);
        mail.To.Add(recipientEmail);
        mail.Subject = "WINDOWS (Matthias) Data from Bandit";
        mail.Body = logData;

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new NetworkCredential(senderEmail, senderPassword) as ICredentialsByHost;
        smtpServer.EnableSsl = true;

        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

        try
        {
            smtpServer.Send(mail);
            Debug.Log("Email sent successfully");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to send email: " + ex.Message);
        }
    }

}
