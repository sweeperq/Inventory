using MimeKit;

namespace Inventory.Email;

public interface IEmailService
{
    Task SendAsync(MimeMessage message, CancellationToken cancellationToken = default);
    Task<bool> CanConnectAsync(CancellationToken cancellationToken = default);
}
