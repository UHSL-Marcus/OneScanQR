using SQLControlsLib;

namespace OneScanWebApp.Database.Objects
{
    public class AdminUser : DatabaseTableObject
    {
        public string Name;
        public int? AdminToken;
    }
}