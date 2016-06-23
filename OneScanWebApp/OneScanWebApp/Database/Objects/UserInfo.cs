using SQLControlsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneScanWebApp.Database.Objects
{
    public class UserInfo : DatabaseTableObject
    {
        public string Name;
        public int? UserToken;
    }
}