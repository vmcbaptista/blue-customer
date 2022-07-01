using BlueCustomer.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCustomer.Core.Tests.ValueObjects
{
    [TestFixture]
    public class NameTests
    {
        [Test]
        public void Constructor_Should_Create_Valid_Name()
        {
            var firstName = "John";
            var surname = "Joe";
            var name = new Name(firstName, surname);

            name.Should().NotBeNull();
            name.FirstName.Should().Be(firstName);
            name.Surname.Should().Be(surname);
        }

        [TestCase("")]
        [TestCase(null)]
        public void Constructor_Should_Prevent_Create_When_First_Name_Is_Not_Valid(string firstName)
        {
            Action t = () => new Name(firstName, "Joe");

            t.Should().Throw<Exception>().WithMessage("First name is invalid");

        }

        [TestCase("")]
        [TestCase(null)]
        public void Constructor_Should_Prevent_Create_When_Surname_Is_Not_Valid(string surname)
        {
            Action t = () => new Name("John", surname);

            t.Should().Throw<Exception>().WithMessage("Surname is invalid");

        }
    }
}
