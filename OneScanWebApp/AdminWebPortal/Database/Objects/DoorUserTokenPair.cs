using SQLControlsLib;

namespace AdminWebPortal.Database.Objects
{
    public class DoorUserTokenPair : DatabaseTableObject
    {
        public int? DoorID;
        public int? UserToken;
    }
}