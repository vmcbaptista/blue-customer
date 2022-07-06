using FluentResults;

namespace BlueCustomer.Core.GeneralErrors
{
    public class ValueIsRequired : Error
    {
        public ValueIsRequired(string argument) : base($"{argument} is required")
        {

        }

    }
}
