using SQLControlsLib;

namespace OneScanWebApp.Database.Objects
{
    public class DoorUserTokenPair : DatabaseTableObject
    {
        public int? DoorID;
        public int? UserToken;
    }
}