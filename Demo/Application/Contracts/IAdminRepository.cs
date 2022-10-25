namespace Demo.Application.Contracts;

public interface IAdminRepository
{
    Task<int> Count(CancellationToken cancellationToken);
}