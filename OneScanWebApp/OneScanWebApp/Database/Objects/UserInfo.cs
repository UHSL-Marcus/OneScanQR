using SQLControlsLib;

namespace OneScanWebApp.Database.Objects
{
    public class UserInfo : DatabaseTableObject
    {
        [DatabaseID]
        public int? Id;
        public string Name;
        public int? UserToken;
    }
}