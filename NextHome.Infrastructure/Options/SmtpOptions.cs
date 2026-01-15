namespace NextHome.Infrastructure.Options;

public class SmtpOptions
{
    /// <summary>
    /// The name of the configuration section for SMTP options.
    /// </summary>
    public const string SectionName = "Smtp";

    /// <summary>
    /// The SMTP server host.
    /// </summary>   
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// The SMTP server port.
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// The username for SMTP authentication.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// The password for SMTP authentication.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// The email address used as the sender.
    /// </summary>
    public string From { get; set; } = string.Empty;
}