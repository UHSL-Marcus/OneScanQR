namespace DoorLockDemoUWP
{
    public class Consts
    {
        public static readonly string URL_BASE = "https://mmtsnap.mmt.herts.ac.uk/onescan/";
        //public static readonly string URL_BASE = "http://5bb49f91.ngrok.io/onescanwebapp/";
        //public static readonly string URL_BASE = "http://localhost/onescanwebapp/";

        private static string _DoorID;
        public static string DoorID
        {
            get
            {
                if (_DoorID == null)
                {
                    var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                    _DoorID = loader.GetString("DoorID");
                }
                return _DoorID;
            }
        }

        private static string _DoorSecret;
        public static string DoorSecret
        {
            get
            {
                if (_DoorSecret == null)
                {
                    var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                    _DoorSecret = loader.GetString("DoorSecret");
                }
                return _DoorSecret;
            }
        }
    }
}