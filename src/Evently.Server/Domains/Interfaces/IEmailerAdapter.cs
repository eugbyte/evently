namespace Evently.Server.Domains.Interfaces;

public interface IEmailerAdapter
{
    // senderEmail: Either actual Sender email or email of the third party that IEmailer sends on behalf of.
    Task SendEmailAsync(string senderEmail, string recipientEmail, string subject, string body);
}
