using SQLControlsLib;

namespace OneScanWebApp.Database.Objects
{
    public class AdminToken : DatabaseTableObject
    {
        [DatabaseID]
        public int? Id;
        public string UserToken;
    }
}