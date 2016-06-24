using SQLControlsLib;

namespace OneScanWebApp.Database.Objects
{
    public class UserInfo : DatabaseTableObject
    {
        public string Name;
        public int? UserToken;
    }
}