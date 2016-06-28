using SQLControlsLib;

namespace OneScanWebApp.Database.Objects
{
    public class AdminUser : DatabaseTableObject
    {
        [DatabaseID]
        public int? Id;
        public string Name;
        public int? AdminToken;
    }
}