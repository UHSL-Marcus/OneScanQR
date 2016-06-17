namespace AdminWebPortal.Views.Main
{
    public partial class UserAdmin : System.Web.UI.Page
    {
        public class TableInfo : SQLControls.DataObject
        {
            public int? UserInfoID;
            public int? UserTokenID;
            public string UserInfoName;
            public string UserTokenToken;

            [SQLControls.SQLIgnore]
            static string SelectSQL = "SELECT UserInfo.Id AS UserInfoID, UserToken.Id AS UserTokenID, UserInfo.Name AS UserInfoName, UserToken.Token AS UserTokenToken FROM UserInfo JOIN UserToken ON UserToken.Id=UserInfo.UserToken";

            public string getSQL()
            {
                return SelectSQL;
            }
        }

        public class DoorViewInfo : SQLControls.DataObject
        {
            public int? Id;
            public string DoorID;

            [SQLControls.SQLIgnore]
            private string UserTokenID;

            [SQLControls.SQLIgnore]
            static string SelectSQL = "SELECT Door.DoorID AS DoorID, Door.Id AS Id FROM Door JOIN DoorUserTokenPair ON Door.Id = DoorUserTokenPair.DoorID WHERE DoorUserTokenPair.UserToken = '@UserTokenID'";

            public DoorViewInfo() { }

            public DoorViewInfo(string UserTokenID)
            {
                this.UserTokenID = UserTokenID;
            }

            public string getSQL()
            {
                return SelectSQL.Replace("@UserTokenID", UserTokenID);
            }
        }

        public class AddDoorInfo : SQLControls.DataObject
        {
            public int? Id;
            public string DoorID;

            [SQLControls.SQLIgnore]
            private string UserTokenID;

            [SQLControls.SQLIgnore]
            static string SelectSQL = "SELECT Door.Id AS Id, Door.DoorID AS DoorID FROM Door WHERE NOT EXISTS (SELECT * FROM DoorUserTokenPair WHERE DoorUserTokenPair.DoorID=Door.Id AND DoorUserTokenPair.UserToken='@UserTokenID')";

            public AddDoorInfo() { }

            public AddDoorInfo(string UserTokenID)
            {
                this.UserTokenID = UserTokenID;
            }
            public string getSQL()
            {
                return SelectSQL.Replace("@UserTokenID", UserTokenID);
            }
        }
    }
}