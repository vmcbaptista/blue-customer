namespace BlueCustomer.Api.Models;

public record UpsertCustomerDto(Guid Id, string FirstName, string Surname, string Email, string Password);

