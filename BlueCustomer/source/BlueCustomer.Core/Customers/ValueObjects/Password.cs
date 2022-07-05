using BlueCustomer.Core.GeneralErrors;
using FluentResults;

namespace BlueCustomer.Core.Customers.ValueObjects;

public record Password
{
    public string Value { get; }

    private Password(string value)
    {
        Value = value;
    }

    public static Result<Password> Create(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return Result.Fail(new ValueIsRequired(nameof(password)));
        }

        return new Password(password);
    }
}

