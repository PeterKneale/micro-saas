using Demo.Application.Contracts;

namespace Demo.Application.Queries;

public static class GetCarByRegistration
{
    public class Query : IRequest<Result>, IRequireTenantContext
    {
        public Query(string registrationNumber)
        {
            RegistrationNumber = registrationNumber;
        }

        public string RegistrationNumber { get; }
    }

    public record Result(Guid Id, string Registration);

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.RegistrationNumber).NotEmpty();
        }
    }

    internal class Handler : IRequestHandler<Query, Result>
    {
        private readonly ICarRepository _cars;

        public Handler(ICarRepository cars)
        {
            _cars = cars;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var registration = Registration.CreateInstance(request.RegistrationNumber);

            var car = await _cars.GetByRegistration(registration, cancellationToken);
            if (car == null)
            {
                throw new CarNotFoundException(request.RegistrationNumber);
            }

            return new Result(car.Id.Id, car.Registration!.RegistrationNumber);
        }
    }
}