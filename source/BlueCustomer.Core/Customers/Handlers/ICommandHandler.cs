using FluentResults;

namespace BlueCustomer.Core.Customers.Handlers;

public interface ICommandHandler<TCommand>
{
    Task<Result> Handle(TCommand command, CancellationToken cancellationToken);
}
