using BlueCustomer.Core.Customers.ValueObjects;
using BlueCustomer.Core.GeneralErrors;
using FluentResults;

namespace BlueCustomer.Core.Customers
{
    public class Customer
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private Customer() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        private Customer(Guid id, Name name, Email email, Password password)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
        }

        public Guid Id { get; }
        public Name Name { get; private set; }
        public Email Email { get; private set; }
        public Password Password { get; private set; }

        public static Result<Customer> Create(Guid id, Name name, Email email, Password password)
        {
            if (id == Guid.Empty) return Result.Fail(new ValueIsInvalid(nameof(id)));
            if (name == null) return Result.Fail(new ValueIsRequired(nameof(name)));
            if (email == null) return Result.Fail(new ValueIsRequired(nameof(email)));
            if (password == null) return Result.Fail(new ValueIsRequired(nameof(password)));

            return new Customer(id, name, email, password);
        }

        public Result Update(Name name, Email email, Password password)
        {
            if (name == null) return Result.Fail(new ValueIsRequired(nameof(name)));
            if (email == null) return Result.Fail(new ValueIsRequired(nameof(email)));
            if (password == null) return Result.Fail(new ValueIsRequired(nameof(password)));

            Name = name;
            Email = email;
            Password = password;

            return Result.Ok();
        }
    }
}
