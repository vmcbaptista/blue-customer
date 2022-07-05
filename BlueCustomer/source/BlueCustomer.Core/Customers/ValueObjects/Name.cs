using BlueCustomer.Core.GeneralErrors;
using FluentResults;

namespace BlueCustomer.Core.Customers.ValueObjects;

public record Name
{
    public string FirstName { get; }
    public string Surname { get; }

    private Name(string firstName, string surname)
    {
        FirstName = firstName;
        Surname = surname;
    }

    public static Result<Name> Create(string firstName, string surname)
    {
        if (string.IsNullOrWhiteSpace(firstName)) return Result.Fail(new ValueIsRequired(nameof(firstName)));
        if (string.IsNullOrWhiteSpace(surname)) return Result.Fail(new ValueIsRequired(nameof(surname)));

        return Result.Ok(new Name(firstName, surname));
    }
}

