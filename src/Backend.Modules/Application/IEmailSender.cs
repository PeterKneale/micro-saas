namespace Backend.Modules.Application;

public interface IEmailSender
{
    Task SendRegisteredEmail(string email, string claimUri, CancellationToken cancellationToken);
    Task SendClaimedEmail(string email, string loginUri, CancellationToken cancellationToken);
}