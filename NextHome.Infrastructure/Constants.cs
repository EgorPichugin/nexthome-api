namespace NextHome.Infrastructure;

/// <summary>
/// Infrastructure constants
/// </summary>
public class Constants
{
    /// <summary>
    /// Template for confirmation email.
    /// </summary>
    public static string ConfirmationEmailTemplate = """
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
    
    /// <summary>
    /// Resend URL to resend email.
    /// </summary>
    public static string ResendUrl = "https://api.resend.com/emails";
    
    /// <summary>
    /// Bearer schema.
    /// </summary>
    public static string Bearer = "Bearer";
}