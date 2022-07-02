using BlueCustomer.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCustomer.Core.Entities
{
    public class Customer
    {
        private Customer() { }

        public Customer(Guid id, Name name, Email email, Password password)
        {
            if (id == Guid.Empty) throw new Exception("Id is invalid");
            if (name == null) throw new Exception("Name is required");
            if (email == null) throw new Exception("Email is required");
            if (password == null) throw new Exception("Password is required");

            Id = id;
            Name = name;
            Email = email;
            Password = password;
        }

        public Guid Id { get; }
        public Name Name { get; private set; }
        public Email Email { get; private set; }
        public Password Password { get;  private set; }

        public void Update(Name name, Email email, Password password)
        {
            if (name == null) throw new Exception("Name is required");
            if (email == null) throw new Exception("Email is required");
            if (password == null) throw new Exception("Password is required");

            Name = name;
            Email = email;
            Password = password;
        }
    }
}
