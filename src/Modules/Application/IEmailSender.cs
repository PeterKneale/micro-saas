namespace Modules.Application;

public interface IEmailSender
{
    Task SendRegisteredEmail(string email, Uri claimUri, CancellationToken cancellationToken);
    Task SendClaimedEmail(string email, Uri loginUri, CancellationToken cancellationToken);
}