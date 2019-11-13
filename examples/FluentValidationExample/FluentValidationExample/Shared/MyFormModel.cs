using System;
using System.Collections.Generic;
using System.Text;

namespace FluentValidationExample.Shared
{
    public class MyFormModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }        

        public string Address { get; set; }

        public int Age { get; set; }
    }
}
