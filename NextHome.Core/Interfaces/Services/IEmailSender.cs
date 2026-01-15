namespace NextHome.Core.Interfaces.Services;

/// <summary>
/// Service is responsible for sending emails.
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Sends an email with a confirmation link.
    /// </summary>
    /// <param name="to">The recipient's email address.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="link">The confirmation link to be included in the email.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendEmail(string to, string subject, string link);
}