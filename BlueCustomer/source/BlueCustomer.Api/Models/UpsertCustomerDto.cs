namespace BlueCustomer.Api.Models;

public record UpsertCustomerDto(Guid Id, string FirstName, string LastName, string Email, string Password);

