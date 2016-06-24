namespace AdminWebPortal.Views.Main
{
    public partial class UserAdmin : System.Web.UI.Page
    {
        private class DoorViewBtnArguments
        {
            public int? UserTokenID;
            public bool showingDoors = false;
        }

        private class AddRegDoorCtlsBtnArguments
        {
            public int? UserTokenID;
            public int? delBtnIndex;
            public bool showingInput = false;
        }
        private class AddDoorRegBtnArguments
        {
            public int? UserTokenID;
            public string DropDownID;
        }
        private class DeleteBtnArguments
        {
            public int? UserTokenID;
            public int? UserInfoID;
            public string RowID;
        }

        private class RemoveAuthBtnArguments
        {
            public int? UserTokenID;
            public int? DoorID;
        }
    }
}