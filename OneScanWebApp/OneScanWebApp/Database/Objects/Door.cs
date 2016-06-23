using SQLControlsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneScanWebApp.Database.Objects
{
    public class Door : DatabaseTableObject
    {
        public string DoorID;
        public string DoorSecret;
    }
}