using SQLControlsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneScanWebApp.Database.Objects
{
    public class Log : DatabaseTableObject
    {
        public string Guid;
        public string Error;
        public DateTime Timestamp;
    }
}