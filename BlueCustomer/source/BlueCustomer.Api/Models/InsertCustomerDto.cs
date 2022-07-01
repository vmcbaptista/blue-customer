namespace BlueCustomer.Api.Models;

public record InsertCustomerDto(Guid Id, string FirstName, string Surname, string Email, string Password);

