using BlueCustomer.Core.Entities;
using BlueCustomer.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlueCustomer.Core.Tests.Entities;

[TestFixture]
public class CustomerTests
{
    [Test]
    public void Constructor_Should_Create_Valid_Customer()
    {
        var id = Guid.NewGuid();
        var name = new Name("John", "Joe");
        var email = new Email("john@joe.com");
        var password = new Password("password");

        var customer = new Customer(id, name, email, password);

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
        var name = new Name("John", "Joe");
        var email = new Email("john@joe.com");
        var password = new Password("password");

        Action t = () => new Customer(id, name, email, password);

        t.Should().Throw<Exception>().WithMessage("Id is invalid");
    }

    [Test]
    public void Constructor_Should_Prevent_Create_When_Name_Is_Null()
    {
        var id = Guid.NewGuid();
        Name name = null;
        var email = new Email("john@joe.com");
        var password = new Password("password");

        Action t = () => new Customer(id, name, email, password);

        t.Should().Throw<Exception>().WithMessage("Name is required");
    }


    [Test]
    public void Constructor_Should_Prevent_Create_When_Email_Is_Null()
    {
        var id = Guid.NewGuid();
        var name = new Name("John", "Joe");
        Email email = null;
        var password = new Password("password");

        Action t = () => new Customer(id, name, email, password);

        t.Should().Throw<Exception>().WithMessage("Email is required");
    }


    [Test]
    public void Constructor_Should_Prevent_Create_When_Password_Is_Null()
    {
        var id = Guid.NewGuid();
        var name = new Name("John", "Joe");
        var email = new Email("john@joe.com");
        Password password = null;

        Action t = () => new Customer(id, name, email, password);

        t.Should().Throw<Exception>().WithMessage("Password is required");
    }
}
