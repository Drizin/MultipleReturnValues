using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MultipleReturnValues.Entities
{
    [DebuggerDisplay("{FirstName,nq} {LastName,nq} ({UserName})")]

    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
    }
}
