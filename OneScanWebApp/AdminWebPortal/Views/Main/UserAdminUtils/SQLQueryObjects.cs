using SQLControls;

namespace AdminWebPortal.Views.Main
{
    public partial class UserAdmin : System.Web.UI.Page
    {
        public class TableInfo : DatabaseOutputObject
        {
            [DatabaseOutput(columnName = "UserInfo.Id")]
            public int? UserInfoID;
            [DatabaseOutput(columnName = "UserToken.Id")]
            public int? UserTokenID;
            [DatabaseOutput(columnName = "UserInfo.Name")]
            public string UserInfoName;
            [DatabaseOutput(columnName = "UserToken.Token")]
            public string UserTokenToken;

            public TableInfo()
            {
                FROM = "UserInfo";
                joinObjects.Add(new DatabaseOutputObjectJoin(JoinTypes.JOIN, "UserInfo", "UserToken", "UserToken", "Id", JoinOperators.EQUALS));
                joinStrings.Add("JOIN UserToken ON UserInfo.UserToken=UserToken.Id");
            }
        }

        public class DoorViewInfo : DatabaseOutputObject
        {
            [DatabaseOutput(columnName = "Door.Id")]
            public int? Id;
            [DatabaseOutput(columnName = "Door.DoorID")]
            public string DoorID;

            public DoorViewInfo() { }

            public DoorViewInfo(string UserTokenID)
            {
                whereObjects.Add(createWhereObject("DoorUserTokenPair", "UserToken", UserTokenID));
                FROM = "Door";
                joinStrings.Add("JOIN DoorUserTokenPair ON DoorUserTokenPair.DoorID=Door.Id");
            }
        }

        public class AddDoorInfo : DatabaseOutputObject
        {
            [DatabaseOutput(columnName = "Door.Id")]
            public int? Id;
            [DatabaseOutput(columnName = "Door.DoorID")]
            public string DoorID;

            static string SelectSQL = "SELECT Door.Id AS Id, Door.DoorID AS DoorID FROM Door WHERE NOT EXISTS (SELECT * FROM DoorUserTokenPair WHERE DoorUserTokenPair.DoorID=Door.Id AND DoorUserTokenPair.UserToken='@UserTokenID')";

            public AddDoorInfo() { }

            public AddDoorInfo(string UserTokenID)
            {
                FROM = "Door";
                  
            }
           
        }
    }
}