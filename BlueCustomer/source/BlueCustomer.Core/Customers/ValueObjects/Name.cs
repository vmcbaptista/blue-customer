namespace BlueCustomer.Core.Customers.ValueObjects;

public record Name
{
    public string FirstName { get; }
    public string Surname { get; }

    public Name(string firstName, string surname)
    {
        if (string.IsNullOrWhiteSpace(firstName)) throw new Exception("First name is invalid");
        if (string.IsNullOrWhiteSpace(surname)) throw new Exception("Surname is invalid");

        FirstName = firstName;
        Surname = surname;
    }
}

