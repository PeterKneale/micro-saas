namespace Demo.Application.Contracts;

public interface ICarRepository
{
    Task<Car?> Get(CarId id, CancellationToken cancellationToken);

    Task<Car?> GetByRegistration(Registration registration, CancellationToken cancellationToken);

    Task<IEnumerable<Car>> List(CancellationToken cancellationToken);

    Task Insert(Car car, CancellationToken cancellationToken);

    Task Update(Car car, CancellationToken cancellationToken);
}