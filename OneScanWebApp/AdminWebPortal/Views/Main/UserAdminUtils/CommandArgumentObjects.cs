using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminWebPortal.Views.Main
{
    public partial class UserAdmin : System.Web.UI.Page
    {
        private class DoorViewBtnArguments
        {
            public int? RowIndex;
            public int? CellIndex;
            public int? UserTokenID;
            public int? DoorTableIndex;
            public bool showingDoors = false;
        }
        private class DeleteBtnArguments
        {
            public int? UserTokenID;
            public int? UserInfoID;
            public int? RowIndex;
        }

        private class RemoveAuthBtnArguments
        {
            public int? UserTokenID;
            public string DoorTableID;
            public string DoorTableRowID;
        }
    }
}