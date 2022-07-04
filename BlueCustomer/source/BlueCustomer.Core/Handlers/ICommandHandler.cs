using FluentResults;

namespace BlueCustomer.Core.Handlers;

public interface ICommandHandler<TCommand>
{
    Task<Result> Handle(TCommand command, CancellationToken cancellationToken);
}
