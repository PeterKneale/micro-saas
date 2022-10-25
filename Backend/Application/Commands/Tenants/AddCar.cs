using Backend.Application.Contracts;
using Backend.Application.Contracts.Tenants;
using Backend.Application.Exceptions;
using Backend.Domain.CarAggregate;

namespace Backend.Application.Commands.Tenants;

public static class AddCar
{
    public class Command : IRequest, IRequireTenantContext
    {
        public Command(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }

    internal class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    internal class Handler : IRequestHandler<Command>
    {
        private readonly ICarRepository _repository;

        public Handler(ICarRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var carId = CarId.CreateInstance(request.Id);

            var exists = await _repository.Get(carId, cancellationToken);
            if (exists != null)
            {
                throw new CarAlreadyExistsException(request.Id);
            }

            var car = Car.CreateInstance(carId);

            await _repository.Insert(car, cancellationToken);

            return Unit.Value;
        }
    }
}