using System.ComponentModel.DataAnnotations;

namespace Inventory.Email;

public class EmailSettings
{
    [Required]
    public string Host { get; set; } = "localhost";

    [Range(1, 65535)]
    public int Port { get; set; }

    [AllowedValues(["None", "Auto", "SslOnConnect", "StartTls", "StartTlsWhenAvailable"],
        ErrorMessage = "{0} must be one of: 'None', 'Auto', 'SslOnConnect', 'StartTls', 'StartTlsWhenAvailable")]
    public string SocketOptions { get; set; } = "StartTls";

    public string? Username { get; set; }

    public string? Password { get; set; }

    [Required, EmailAddress]
    public string DefaultFromEmail { get; set; } = "no-reply@yourdomain.com";

    public string? DefaultFromName { get; set; }
}
