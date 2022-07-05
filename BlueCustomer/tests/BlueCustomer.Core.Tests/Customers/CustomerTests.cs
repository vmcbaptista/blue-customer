using BlueCustomer.Core.Customers;
using BlueCustomer.Core.Customers.ValueObjects;
using BlueCustomer.Core.GeneralErrors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlueCustomer.Core.Tests.Customers;

[TestFixture]
public class CustomerTests
{
    [Test]
    public void Create_Should_Create_Valid_Customer()
    {
        var id = Guid.NewGuid();
        var name = Name.Create("John", "Joe").Value;
        var email = Email.Create("john@joe.com").Value;
        var password = Password.Create("password").Value;

        var customer = Customer.Create(id, name, email, password).Value;

        customer.Should().NotBeNull();
        customer.Id.Should().Be(id);
        customer.Name.Should().Be(name);
        customer.Email.Should().Be(email);
        customer.Password.Should().Be(password);
    }

    [Test]
    public void Constructor_Should_Prevent_Create_When_Id_Is_Invalid()
    {
        var id = Guid.Empty;
        var name = Name.Create("John", "Joe").Value;
        var email = Email.Create("john@joe.com").Value;
        var password = Password.Create("password").Value;

        var createResult = Customer.Create(id, name, email, password);

        createResult.IsFailed.Should().BeTrue();
        createResult.HasError<ValueIsInvalid>().Should().BeTrue();
        (createResult.Errors[0] as ValueIsInvalid).Message.Should().Be("id is invalid");
    }

    [Test]
    public void Constructor_Should_Prevent_Create_When_Name_Is_Null()
    {
        var id = Guid.NewGuid();
        Name name = null;
        var email = Email.Create("john@joe.com").Value;
        var password = Password.Create("password").Value;

        var createResult = Customer.Create(id, name, email, password);

        createResult.IsFailed.Should().BeTrue();
        createResult.HasError<ValueIsRequired>().Should().BeTrue();
        (createResult.Errors[0] as ValueIsRequired).Message.Should().Be("name is required");
    }


    [Test]
    public void Constructor_Should_Prevent_Create_When_Email_Is_Null()
    {
        var id = Guid.NewGuid();
        var name = Name.Create("John", "Joe").Value;
        Email email = null;
        var password = Password.Create("password").Value;

        var createResult = Customer.Create(id, name, email, password);

        createResult.IsFailed.Should().BeTrue();
        createResult.HasError<ValueIsRequired>().Should().BeTrue();
        (createResult.Errors[0] as ValueIsRequired).Message.Should().Be("email is required");
    }


    [Test]
    public void Constructor_Should_Prevent_Create_When_Password_Is_Null()
    {
        var id = Guid.NewGuid();
        var name = Name.Create("John", "Joe").Value;
        var email = Email.Create("john@joe.com").Value;
        Password password = null;

        var createResult = Customer.Create(id, name, email, password);

        createResult.IsFailed.Should().BeTrue();
        createResult.HasError<ValueIsRequired>().Should().BeTrue();
        (createResult.Errors[0] as ValueIsRequired).Message.Should().Be("password is required");
    }
}
