

using AdminWebPortal.Database.Objects;
using SQLControlsLib;

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
                //"FROM Door FULL OUTER JOIN DoorUserTokenPair on DoorUserTokenPair.DoorID<>Door.Id where DoorUserTokenPair.UserToken<>3 or DoorUserTokenPair.UserToken is NULL";
                // FROM Door FULL OUTER JOIN DoorUserTokenPair ON Door.Id<>DoorUserTokenPair.DoorID  WHERE (DoorUserTokenPair.UserToken<>18) OR (DoorUserTokenPair.UserToken IS NULL)

                JoinOnPair on = new JoinOnPair("Id", "DoorID");
                JoinPair join = new JoinPair(typeof(Door).Name, typeof(DoorUserTokenPair).Name, new JoinOnPair[] { on }, JoinTypes.FULLJOIN);
                joins.Add(join);

                DoorUserTokenPair dtp = new DoorUserTokenPair();
                dtp.UserToken = int.Parse(UserTokenID);
                dtp.setFieldOptions("UserToken", SQLEqualityOperations.NOTEQUALS);
                whereobs.Add(new whereObject(dtp));

                DoorUserTokenPair dtp2 = new DoorUserTokenPair();
                dtp2.setFieldOptions("UserToken", true);
                whereobs.Add(new whereObject(dtp2, SQLWhereConjuctions.OR));

            }
        }
    }
}