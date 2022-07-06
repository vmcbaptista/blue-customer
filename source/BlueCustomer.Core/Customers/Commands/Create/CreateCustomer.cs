namespace BlueCustomer.Core.Customers.Commands.Create
{

    public record CreateCustomer(Guid Id, string FirstName, string Surname, string Email, string Password);
}
