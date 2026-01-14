using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using NextHome.Core.Interfaces.Services;
using NextHome.Infrastructure.Options;

namespace NextHome.Infrastructure.Services;

/// <summary>
/// Service is responsible for sending emails.
/// </summary>
/// <param name="options">The SMTP options for configuring the email sender.</param>
public class SmtpEmailSender(IOptions<SmtpOptions> options) : IEmailSender
{
    /// <summary>
    /// The email template for confirmation emails.
    /// </summary>
    private const string Template = """
        <p>Hello,</p>
        <p>Please confirm your email:</p>

        <a href="{0}"
           style="display:inline-block;
                  padding:12px 20px;
                  background:#4CAF50;
                  color:#fff;
                  text-decoration:none;
                  border-radius:6px;">
           Confirm email
        </a>

        <p>If the button does not work, use this link:</p>
        <p>{0}</p>
        """;

    /// <inheritdoc/>
    public async Task SendEmail(string to, string subject, string link)
    {
        var smtp = options.Value;

        using var client = new SmtpClient(smtp.Host, smtp.Port)
        {
            Credentials = new NetworkCredential(
                smtp.Username,
                smtp.Password),
            EnableSsl = true
        };

        using var message = new MailMessage
        {
            From = new MailAddress(smtp.From),
            Subject = subject,
            Body = string.Format(Template, link),
            IsBodyHtml = true
        };

        message.To.Add(to);

        await client.SendMailAsync(message);
    }
}
