using System;
using System.Threading.Tasks;
using ChildCareApplication.Domain;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

public class EmailHandler : IEmailSender
{
    private readonly SmtpSettings _smtpSettings;

    public EmailHandler(SmtpSettings smtpSettings)
    {
        _smtpSettings = smtpSettings;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlMessage
        };
        emailMessage.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, SecureSocketOptions.StartTls);

            // Remove XOAUTH2 to avoid using OAuth2 authentication
            client.AuthenticationMechanisms.Remove("XOAUTH2");

            var smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
            if (smtpPassword == null)
            {
                throw new InvalidOperationException("SMTP password is not set in environment variables.");
            }

            await client.AuthenticateAsync(_smtpSettings.Username, smtpPassword);
            await client.SendAsync(emailMessage);
        }
        catch (Exception ex)
        {
           throw new InvalidOperationException("Failed to send email.", ex);
           
        }
        finally
        {
           await client.DisconnectAsync(true);
        }
    }
}
