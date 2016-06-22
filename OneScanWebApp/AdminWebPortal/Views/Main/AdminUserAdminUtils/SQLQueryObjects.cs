using SQLControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminWebPortal.Views.Main
{
    public partial class AdminUserAdmin : System.Web.UI.Page
    {
        public class TableInfo : DatabaseOutputObject
        {
            [DatabaseColumn(columnName = "AdminUser.Id")]
            public int AdminUserId;
            [DatabaseColumn(columnName = "AdminToken.Id")]
            public int AdminTokenId;
            [DatabaseColumn(columnName = "AdminUser.Name")]
            public string AdminUser_Name;
            [DatabaseColumn(columnName = "AdminToken.UserToken")]
            public string AdminToken_UserToken;
            
            public TableInfo()
            {
                buildSingleJoin("AdminUser", "AdminToken", "AdminToken", "Id");
            }
        }
    }
}