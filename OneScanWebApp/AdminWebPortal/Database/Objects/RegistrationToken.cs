using SQLControlsLib;

namespace AdminWebApp.Database.Objects
{
    public class RegistrationToken : DatabaseTableObject
    {
        public string AuthKey;
        public string Secret;
    }
}