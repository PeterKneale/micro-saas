﻿using Backend.Application.Contracts.Admin;
using Backend.Application.Exceptions;
using Backend.Domain.TenantAggregate;

namespace Backend.Application.Queries.Admin;

public static class GetTenant
{
    public record Query(Guid Id) : IRequest<Result>;

    public record Result(Guid Id, string Name, string Identifier);

    internal class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    internal class Handler : IRequestHandler<Query, Result>
    {
        private readonly IManagementRepository _repository;

        public Handler(IManagementRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var tenantId = TenantId.CreateInstance(request.Id);

            var tenant = await _repository.Get(tenantId, cancellationToken);
            if (tenant == null)
            {
                throw new TenantNotFoundException(request.Id);
            }

            return new Result(tenant.Id.Id, tenant.Name.Value, tenant.Identifier.Value);
        }
    }
}