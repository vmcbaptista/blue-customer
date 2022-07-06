using BlueCustomer.Core.Customers.ValueObjects;
using BlueCustomer.Core.GeneralErrors;

namespace BlueCustomer.Core.Tests.Customers.ValueObjects
{
    [TestFixture]
    public class NameTests
    {
        [Test]
        public void Create_Should_Create_Valid_Name()
        {
            var firstName = "John";
            var surname = "Joe";
            var name = Name.Create(firstName, surname).Value;

            name.Should().NotBeNull();
            name.FirstName.Should().Be(firstName);
            name.Surname.Should().Be(surname);
        }

        [TestCase("")]
        [TestCase(null)]
        public void Create_Should_Prevent_Create_When_First_Name_Is_Not_Valid(string firstName)
        {
            var createResult = Name.Create(firstName, "Joe");

            createResult.IsFailed.Should().BeTrue();
            createResult.HasError<ValueIsRequired>().Should().BeTrue();
            (createResult.Errors[0] as ValueIsRequired).Message.Should().Be("firstName is required");

        }

        [TestCase("")]
        [TestCase(null)]
        public void Create_Should_Prevent_Create_When_Surname_Is_Not_Valid(string surname)
        {
            var createResult = Name.Create("John", surname);

            createResult.IsFailed.Should().BeTrue();
            createResult.HasError<ValueIsRequired>().Should().BeTrue();
            (createResult.Errors[0] as ValueIsRequired).Message.Should().Be("surname is required");

        }
    }
}
