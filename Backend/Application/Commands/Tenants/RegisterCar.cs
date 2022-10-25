using Backend.Application.Contracts;
using Backend.Application.Contracts.Tenants;
using Backend.Application.Exceptions;
using Backend.Domain.CarAggregate;

namespace Backend.Application.Commands.Tenants;

public static class RegisterCar
{
    public class Command : IRequest, IRequireTenantContext
    {
        public Command(Guid id, string registration)
        {
            Id = id;
            Registration = registration;
        }

        public Guid Id { get; }
        public string Registration { get; }
    }

    internal class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Registration).NotEmpty();
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
            var registration = Registration.CreateInstance(request.Registration);

            var exists = await _repository.GetByRegistration(registration, cancellationToken);
            if (exists != null)
            {
                if (exists.Id.Id.Equals(carId.Id))
                {
                    throw new CarAlreadyRegisteredException(request.Registration);
                }
                throw new CarRegistrationUnavailableException(request.Registration);
            }

            var car = await _repository.Get(carId, cancellationToken);
            if (car == null)
            {
                throw new CarBaseNotFoundException(request.Registration);
            }
            if (car.Registration != null)
            {
                throw new CarAlreadyRegisteredException(request.Registration);
            }

            car.Register(registration);

            await _repository.Update(car, cancellationToken);

            return Unit.Value;
        }
    }
}