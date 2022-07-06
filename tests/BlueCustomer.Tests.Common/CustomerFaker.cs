using BlueCustomer.Core.Customers;
using BlueCustomer.Core.Customers.ValueObjects;
using Bogus;

namespace BlueCustomer.Tests.Common
{
    public class CustomerFaker : Faker<Customer>
    {
        public CustomerFaker()
        {
            CustomInstantiator(f => Customer.Create(f.Random.Guid(), Name.Create(f.Random.String(), f.Random.String()).Value, Email.Create(f.Internet.Email()).Value, Password.Create(f.Random.String()).Value).Value);
        }
    }
}
