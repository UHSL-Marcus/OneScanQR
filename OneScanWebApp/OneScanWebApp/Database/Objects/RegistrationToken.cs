using SQLControlsLib;

namespace OneScanWebApp.Database.Objects
{
    public class RegistrationToken : DatabaseTableObject
    {
        public string AuthKey;
        public string Secret;
    }
}