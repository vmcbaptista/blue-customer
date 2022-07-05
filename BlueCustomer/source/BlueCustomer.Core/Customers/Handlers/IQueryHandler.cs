using FluentResults;

namespace BlueCustomer.Core.Customers.Handlers;

public interface IQueryHandler<T, TResult>
{
    Task<Result<TResult>> Handle(T query, CancellationToken cancellationToken);
}