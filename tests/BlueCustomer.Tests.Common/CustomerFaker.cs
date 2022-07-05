using BlueCustomer.Core.Customers;
using BlueCustomer.Core.Customers.ValueObjects;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
