﻿using Backend.Application.Contracts;
using Backend.Application.Contracts.Tenants;
using Backend.Application.Exceptions;
using Backend.Domain.CarAggregate;

namespace Backend.Application.Queries.Tenants;

public static class GetCar
{
    public class Query : IRequest<Result>, IRequireTenantContext
    {
        public Query(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }

    public record Result(Guid Id, string? Registration);

    internal class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
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
            var carId = CarId.CreateInstance(request.Id);

            var car = await _cars.Get(carId, cancellationToken);
            if (car == null)
            {
                throw new CarBaseNotFoundException(request.Id);
            }

            return new Result(car.Id.Id, car.Registration?.RegistrationNumber);
        }
    }
}