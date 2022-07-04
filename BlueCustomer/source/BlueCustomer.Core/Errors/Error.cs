using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCustomer.Core.Errors
{
    public class DomainErrors
    {
        public class Customer
        {
            public class NotFound : Error
            {
                public NotFound() : base("Customer not found")
                {
                }
            }
        }
    }
}
