

using AdminWebPortal.Database.Objects;
using SQLControls;

namespace AdminWebPortal.Views.Main
{
    public partial class UserAdmin : System.Web.UI.Page
    {
        public class TableInfo : DatabaseOutputObject
        {
            [DatabaseColumn(columnName = "UserInfo.Id")]
            public int? UserInfoID;
            [DatabaseColumn(columnName = "UserToken.Id")]
            public int? UserTokenID;
            [DatabaseColumn(columnName = "UserInfo.Name")]
            public string UserInfoName;
            [DatabaseColumn(columnName = "UserToken.Token")]
            public string UserTokenToken;

            public TableInfo()
            {
                buildSingleJoin("UserInfo", "UserToken", "UserToken", "Id");
            } 
        }

        public class DoorViewInfo : DatabaseOutputObject
        {
            [DatabaseColumn(columnName = "Door.Id")]
            public int? Id;
            [DatabaseColumn(columnName = "Door.DoorID")]
            public string DoorID;

            public DoorViewInfo() { }

            public DoorViewInfo(string UserTokenID)
            {
                buildSingleJoin("DoorUserTokenPair", "Door", "DoorID", "Id");
                buildSingleWhere("DoorUserTokenPair", "UserToken", UserTokenID);
            }
        }

        public class AddDoorInfo : DatabaseOutputObject
        {
            [DatabaseColumn(columnName = "Door.Id")]
            public int? Id;
            [DatabaseColumn(columnName = "Door.DoorID")]
            public string DoorID;

            public AddDoorInfo() { }

            public AddDoorInfo(string UserTokenID)
            {
                "FROM Door FULL OUTER JOIN DoorUserTokenPair on DoorUserTokenPair.DoorID<>Door.Id where DoorUserTokenPair.UserToken<>3 or DoorUserTokenPair.UserToken is NULL";

                JoinOnPair on = new JoinOnPair("Id", "DoorID", "<>");
                JoinPair join = new JoinPair(typeof(Door), typeof(DoorUserTokenPair), new JoinOnPair[] { on });

                DoorUserTokenPair dtp = new DoorUserTokenPair();
                dtp.DoorID.ChangeFieldEquality<DoorUserTokenPair>(SQLEqualityOperations.ISNULL);
            }
        }
    }
}