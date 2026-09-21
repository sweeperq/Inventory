using MimeKit;

namespace Inventory.Email;

public class EmailBuilder
{
    private readonly MimeMessage _emailMessage = new();
    private readonly BodyBuilder _bodyBuilder = new();
    private bool _built = false;

    public static EmailBuilder Create() => new();

    private EmailBuilder PerformAction(Action action)
    {
        EnsureNotBuilt();
        action();
        return this;
    }

    public EmailBuilder AddFrom(string email, string? name = null)
        => PerformAction(() => _emailMessage.From.Add(new MailboxAddress(name, email)));

    public EmailBuilder AddReplyTo(string email, string? name = null)
        => PerformAction(() => _emailMessage.ReplyTo.Add(new MailboxAddress(name, email)));

    public EmailBuilder AddTo(string email, string? name = null)
        => PerformAction(() => _emailMessage.To.Add(new MailboxAddress(name, email)));

    public EmailBuilder AddCc(string email, string? name = null)
        => PerformAction(() => _emailMessage.Cc.Add(new MailboxAddress(name, email)));

    public EmailBuilder AddBcc(string email, string? name = null)
        => PerformAction(() => _emailMessage.Bcc.Add(new MailboxAddress(name, email)));

    public EmailBuilder SetSubject(string subject)
        => PerformAction(() => _emailMessage.Subject = subject);

    public EmailBuilder SetHtmlBody(string htmlBody)
        => PerformAction(() => _bodyBuilder.HtmlBody = htmlBody);

    public EmailBuilder SetTextBody(string textBody)
        => PerformAction(() => _bodyBuilder.TextBody = textBody);

    public EmailBuilder SetImportance(MessageImportance importance)
        => PerformAction(() => _emailMessage.Importance = importance);

    public EmailBuilder AddAttachment(string filePath)
        => PerformAction(() => _bodyBuilder.Attachments.Add(filePath));

    public EmailBuilder AddAttachment(string fileName, byte[] content)
        => PerformAction(() => _bodyBuilder.Attachments.Add(fileName, content));

    public EmailBuilder AddAttachment(string fileName, Stream content)
        => PerformAction(async () => {
            ArgumentNullException.ThrowIfNull(content);
            using var ms = new MemoryStream();
            await content.CopyToAsync(ms);
            _bodyBuilder.Attachments.Add(fileName, ms.ToArray());
        });

    public EmailBuilder AddAttachment(MimeEntity attachment)
        => PerformAction(() => _bodyBuilder.Attachments.Add(attachment));

    public MimeMessage Build()
    {
        EnsureNotBuilt();

        if (_emailMessage.To.Count == 0 && _emailMessage.Cc.Count == 0 && _emailMessage.Bcc.Count == 0)
        {
            throw new InvalidOperationException("At least one recipient (To, Cc, or Bcc) must be specified.");
        }

        _emailMessage.Body = _bodyBuilder.ToMessageBody();
        _built = true;
        return _emailMessage;
    }

    private void EnsureNotBuilt()
    {
        if (_built)
        {
            throw new InvalidOperationException("Message has already been built.");
        }
    }
}
