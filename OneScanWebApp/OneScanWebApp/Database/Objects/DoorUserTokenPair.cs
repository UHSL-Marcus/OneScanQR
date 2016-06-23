using SQLControlsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneScanWebApp.Database.Objects
{
    public class DoorUserTokenPair : DatabaseTableObject
    {
        public int? DoorID;
        public int? UserToken;
    }
}