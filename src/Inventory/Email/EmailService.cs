using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Inventory.Email;

public class EmailService(IOptions<EmailSettings> settings, ILogger<EmailService> logger) : IEmailService
{
    private readonly EmailSettings _settings = settings.Value;
    private readonly SecureSocketOptions _secureSocketOptions = Enum.Parse<SecureSocketOptions>(settings.Value.SocketOptions, true);

    public async Task SendAsync(MimeMessage message, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        if (message.To.Count == 0 && message.Cc.Count == 0 && message.Bcc.Count == 0)
            throw new ArgumentException("Email message must have at least one recipient in To, Cc, or Bcc.", nameof(message));

        if (message.From.Count == 0)
            message.From.Add(new MailboxAddress(_settings.DefaultFromName, _settings.DefaultFromEmail));

        using var client = new SmtpClient();
        try
        {
            await ConnectAndAuthenticateAsync(client, cancellationToken);
            await client.SendAsync(message, cancellationToken);
        }
        catch (Exception ex)
        {
            var recipients = string.Join(", ", message.GetRecipients().Select(r => r.Address));
            logger.LogError(ex, "Failed to send email to {Recipients} via SMTP server {Host}:{Port}", recipients, _settings.Host, _settings.Port);
            throw;
        }
        finally
        {
            if (client.IsConnected)
            {
                await client.DisconnectAsync(true, cancellationToken);
            }
        }
    }

    public async Task<bool> CanConnectAsync(CancellationToken cancellationToken = default)
    {
        using var client = new SmtpClient();
        try
        {
            await ConnectAndAuthenticateAsync(client, cancellationToken);
            return true;
        }
        catch(Exception ex)
        {
            logger.LogWarning(ex, "Failed to connect and authenticate to SMTP server {Host}:{Port}", _settings.Host, _settings.Port);
            return false;
        }
        finally
        {
            if (client.IsConnected)
            { 
                await client.DisconnectAsync(true, cancellationToken);
            }
        }
    }

    private async Task ConnectAndAuthenticateAsync(SmtpClient client, CancellationToken cancellationToken = default)
    {
        await client.ConnectAsync(_settings.Host, _settings.Port, _secureSocketOptions, cancellationToken);
        if (!string.IsNullOrEmpty(_settings.Username))
        { 
            await client.AuthenticateAsync(_settings.Username, _settings.Password ?? string.Empty, cancellationToken);
        }
    }
}
