using SQLControlsLib;
using System;

namespace OneScanWebApp.Database.Objects
{
    public class Log : DatabaseTableObject
    {
        public string Guid;
        public string Error;
        public DateTime Timestamp;
    }
}