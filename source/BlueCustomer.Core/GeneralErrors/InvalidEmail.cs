using FluentResults;

namespace BlueCustomer.Core.GeneralErrors
{
    public class ValueIsInvalid : Error
    {
        public ValueIsInvalid(string argument) : base($"{argument} is invalid")
        {

        }
    }
}
