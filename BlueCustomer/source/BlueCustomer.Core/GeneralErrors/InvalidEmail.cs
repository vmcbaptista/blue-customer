using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCustomer.Core.GeneralErrors
{
    public class ValueIsInvalid : Error
    {
        public ValueIsInvalid(string argument) : base($"{argument} is invalid")
        {

        }
    }
}
