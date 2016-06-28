using SQLControlsLib;

namespace OneScanWebApp.Database.Objects
{
    public class RegistrationToken : DatabaseTableObject
    {
        [DatabaseID]
        public int? Id;
        public string AuthKey;
        public string Secret;
    }
}