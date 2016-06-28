using SQLControlsLib;

namespace OneScanWebApp.Database.Objects
{
    public class Door : DatabaseTableObject
    {
        [DatabaseID]
        public int? Id;
        public string DoorID;
        public string DoorSecret;
    }
}