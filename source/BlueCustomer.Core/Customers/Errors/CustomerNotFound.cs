using FluentResults;

namespace BlueCustomer.Core.Customers.Errors
{
    public class CustomerNotFound : Error
    {
        public CustomerNotFound() : base("Customer not found")
        {
        }
    }
}
