using System.ComponentModel.DataAnnotations;

namespace Career635.Areas.Admin.Models;

public record EnableTwoFactorViewModel(
    string SharedKey,
    string AuthenticatorUri, // For QR Code
    string? VerificationCode
);


/// <summary>
/// Model used for the 2FA security challenge during the sign-in process.
/// </summary>
public class VerifyTwoFactorViewModel
{
    /// <summary>
    /// The 6-digit Time-based One-Time Password (TOTP) from the user's authenticator app.
    /// </summary>
    [Required(ErrorMessage = "Verification code is required.")]
    [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Text)]
    [Display(Name = "Authenticator Code")]
    public string VerificationCode { get; set; } = string.Empty;

    /// <summary>
    /// Carried over from the initial login screen to determine if the 
    /// persistent session cookie should be created.
    /// </summary>
    public bool RememberMe { get; set; }

    /// <summary>
    /// If true, the system will set a "Recovery Cookie" on this browser 
    /// to bypass the 2FA challenge for the next 30 days.
    /// </summary>
    [Display(Name = "Trust this device?")]
    public bool RememberDevice { get; set; }

    /// <summary>
    /// The URL the user was attempting to access before being challenged.
    /// </summary>
    public string? ReturnUrl { get; set; }
}