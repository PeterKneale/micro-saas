using Backend.Application.Contracts;
using Backend.Application.Contracts.Admin;
using Backend.Application.Exceptions;
using Backend.Domain.TenantAggregate;

namespace Backend.Application.Commands.Admin;

public static class AddTenant
{
    public class Command : IRequest
    {
        public Command(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }
    }

    internal class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    internal class Handler : IRequestHandler<Command>
    {
        private readonly IManagementRepository _repository;

        public Handler(IManagementRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var tenantId = TenantId.CreateInstance(request.Id);

            var exists = await _repository.Get(tenantId, cancellationToken);
            if (exists != null)
            {
                throw new TenantAlreadyExistsException(request.Id);
            }
            
            var name = Name.CreateInstance(request.Name);
            var tenant =  Tenant.CreateInstance(tenantId, name);

            await _repository.Insert(tenant, cancellationToken);

            return Unit.Value;
        }
    }
}