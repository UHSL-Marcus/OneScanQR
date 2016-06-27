using SQLControlsLib;

namespace AdminWebApp.Database.Objects
{
    public class RegistrationToken : DatabaseTableObject
    {
        [DatabaseID]
        public int? Id;
        public string AuthKey;
        public string Secret;
    }
}