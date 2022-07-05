namespace BlueCustomer.Core.Customers.Commands.Update
{

    public record UpdateCustomer(Guid Id, string FirstName, string Surname, string Email, string Password);
}
