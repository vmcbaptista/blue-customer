using BlueCustomer.Core.Customers.ValueObjects;

namespace BlueCustomer.Core.Customers
{
    public class Customer
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private Customer() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public Customer(Guid id, Name name, Email email, Password password)
        {
            if (id == Guid.Empty) throw new Exception("Id is invalid");
            if (name == null) throw new Exception("Name is required");
            if (email == null) throw new Exception("Email is required");
            if (password == null) throw new Exception("Password is required");

            Id = id;
            Name = name;
            Email = email;
            Password = password;
        }

        public Guid Id { get; }
        public Name Name { get; private set; }
        public Email Email { get; private set; }
        public Password Password { get; private set; }

        public void Update(Name name, Email email, Password password)
        {
            if (name == null) throw new Exception("Name is required");
            if (email == null) throw new Exception("Email is required");
            if (password == null) throw new Exception("Password is required");

            Name = name;
            Email = email;
            Password = password;
        }
    }
}
