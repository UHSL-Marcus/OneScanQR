using SQLControlsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneScanWebApp.Database.Objects
{
    public class RegistrationToken : DatabaseTableObject
    {
        public string AuthKey;
        public string Secret;
    }
}