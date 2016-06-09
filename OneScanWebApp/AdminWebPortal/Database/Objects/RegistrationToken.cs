using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminWebApp.Database.Objects
{
    public class RegistrationToken
    {
        public int? Id;
        public string AuthKey;
        public string Secret;
    }
}