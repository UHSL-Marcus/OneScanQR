using SQLControlsLib;

namespace OneScanWebApp.Database.Objects
{
    public class UserToken : DatabaseTableObject
    {
        [DatabaseID]
        public int? Id;
        public string Token;
    }
}