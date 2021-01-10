using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MultipleReturnValues.Entities
{
    [System.Diagnostics.DebuggerDisplay("{ToString()}")]

    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName} ({UserName})";
        }
    }
}
