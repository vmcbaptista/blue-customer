using BlueCustomer.Core.GeneralErrors;
using FluentResults;
using System.Text.RegularExpressions;

namespace BlueCustomer.Core.Customers.ValueObjects;

public record Email
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Fail(new ValueIsRequired(nameof(email)));
        }

        if (Regex.IsMatch(email, @"^(.+)@(.+)$") == false)
        {
            return Result.Fail(new ValueIsInvalid(nameof(email)));
        }

        return new Email(email);
    }
}

