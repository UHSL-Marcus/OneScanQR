using AdminWebPortal.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AdminWebPortal.Views.Main
{
    public partial class UserAdmin : System.Web.UI.Page
    {
        private void fillTable()
        {
            TableRow headings = usersTbl.Rows[0];
            usersTbl.Rows.Clear();
            usersTbl.Rows.Add(headings);

            List<TableInfo> info;
            if (SQLControlsLib.Get.doJoinSelect(new TableInfo(), out info))
            {
                foreach (TableInfo entry in info)
                {
                    TableRow tRow = new TableRow();
                    tRow.ID = "UsersTableRow" + entry.UserTokenID.Value;

                    TableCell tCell = new TableCell();
                    tCell.Text = entry.UserInfoName;
                    tRow.Cells.Add(tCell);

                    tCell = new TableCell();
                    tCell.Text = entry.UserTokenToken;
                    tRow.Cells.Add(tCell);

                    tCell = new TableCell();
                    tCell.ID = "UsersTableActionsCell" + entry.UserTokenID.Value;

                    Button doorViewBtn = new Button();
                    doorViewBtn.Text = "Show Registered Doors";
                    doorViewBtn.ID = "ShowRegisteredDoors" + entry.UserTokenID;
                    doorViewBtn.Click += DoorViewBtn_Click;
                    DoorViewBtnArguments doorViewBtnArgs = new DoorViewBtnArguments();
                    doorViewBtnArgs.UserTokenID = entry.UserTokenID;

                    tCell.Controls.Add(doorViewBtn);

                    Button addDoorBtn = new Button();
                    addDoorBtn.Text = "Register To Door";
                    addDoorBtn.ID = "RegisterToDoor" + entry.UserTokenID;
                    addDoorBtn.Click += AddRegDoorCtlsBtn_Click;
                    AddRegDoorCtlsBtnArguments addDoorBtnArgs = new AddRegDoorCtlsBtnArguments();
                    addDoorBtnArgs.UserTokenID = entry.UserTokenID;

                    tCell.Controls.Add(addDoorBtn);

                    Button delBtn = new Button();
                    delBtn.Text = "Delete User";
                    delBtn.ID = "deleteUser" + entry.UserTokenID;
                    delBtn.Click += DelBtn_Click;
                    DeleteBtnArguments deletebtnArgs = new DeleteBtnArguments();
                    deletebtnArgs.UserInfoID = entry.UserInfoID;
                    deletebtnArgs.UserTokenID = entry.UserTokenID;

                    tCell.Controls.Add(delBtn);

                    if (OpenDoorRegisterCtls.ContainsKey(entry.UserTokenID.Value))
                        addDoorRegisterControls(tCell.Controls, entry.UserTokenID.Value, OpenDoorRegisterCtls[entry.UserTokenID.Value].Item1);


                    if (OpenDoorTables.ContainsKey(entry.UserTokenID.Value))
                        addDoorViewTable(tCell.Controls, entry.UserTokenID.Value);


                    tRow.Cells.Add(tCell);

                    usersTbl.Rows.Add(tRow);

                    addDoorBtnArgs.delBtnIndex = tCell.Controls.IndexOf(delBtn);

                    deletebtnArgs.RowID = tRow.ID;

                    doorViewBtn.CommandArgument = JsonUtils.GetJson(doorViewBtnArgs);
                    addDoorBtn.CommandArgument = JsonUtils.GetJson(addDoorBtnArgs);
                    delBtn.CommandArgument = JsonUtils.GetJson(deletebtnArgs);

                }
            }
        }

        private string addDoorViewTable(ControlCollection col, int userTknID)
        {
            Table DoorTable = new Table();
            DoorTable.ID = "DoorViewTable" + userTknID;
            TableHeaderRow headRow = new TableHeaderRow();

            TableHeaderCell headCell = new TableHeaderCell();
            headCell.Text = "Door ID";
            headRow.Cells.Add(headCell);

            headCell = new TableHeaderCell();
            headCell.Text = "Actions";
            headRow.Cells.Add(headCell);

            DoorTable.Rows.Add(headRow);

            List<DoorViewInfo> info;
            if (SQLControlsLib.Get.doJoinSelect(new DoorViewInfo(userTknID.ToString()), out info))
            {
                foreach (DoorViewInfo entry in info)
                {
                    TableRow tRow = new TableRow();
                    tRow.ID = "DoorViewTableRow" + userTknID + "Door" + entry.DoorID;

                    TableCell tCell = new TableCell();
                    tCell.Text = entry.DoorID;
                    tRow.Cells.Add(tCell);

                    tCell = new TableCell();

                    Button removeDoorAuthBtn = new Button();
                    removeDoorAuthBtn.Text = "Remove Authorisation";
                    removeDoorAuthBtn.ID = "RemoveAuthorisation" + userTknID + "Door" + entry.Id;
                    removeDoorAuthBtn.Click += RemoveDoorAuthBtn_Click;

                    RemoveAuthBtnArguments removeAuthArgs = new RemoveAuthBtnArguments();
                    removeAuthArgs.UserTokenID = userTknID;
                    removeAuthArgs.DoorID = entry.Id;

                    removeDoorAuthBtn.CommandArgument = JsonUtils.GetJson(removeAuthArgs);

                    tCell.Controls.Add(removeDoorAuthBtn);


                    tRow.Cells.Add(tCell);

                    DoorTable.Rows.Add(tRow);
                }
            }

            col.Add(DoorTable);
            return DoorTable.ID;
        }

        private string[] addDoorRegisterControls(ControlCollection col, int userTokenID, int? index = null)
        {
            
            DropDownList dl = new DropDownList();
            dl.ID = "addDoorRegisterDropDown" + userTokenID;

            Button addDoorRegisterBtn = new Button();
            addDoorRegisterBtn.ID = "addDoorRegisterBtn" + userTokenID;
            addDoorRegisterBtn.Text = "Register Door";
            addDoorRegisterBtn.Click += AddDoorRegisterBtn_Click;
            AddDoorRegBtnArguments btnArgs = new AddDoorRegBtnArguments();
            btnArgs.UserTokenID = userTokenID;
            btnArgs.DropDownID = dl.ID;
            addDoorRegisterBtn.CommandArgument = JsonUtils.GetJson(btnArgs);

            LiteralControl brk = new LiteralControl("<br />");
            brk.ID = "addDoorRegisterBreak" + userTokenID;

            List<AddDoorInfo> info;
            if (SQLControlsLib.Get.doJoinSelect(new AddDoorInfo(userTokenID.ToString()), out info))
            { 
                foreach (AddDoorInfo entry in info)
                    dl.Items.Add(new ListItem(entry.DoorID, entry.Id.Value.ToString()));
            }

            if (index == null)
                index = col.Count;

            col.AddAt(index.Value, brk);
            col.AddAt(index.Value+1, dl);
            col.AddAt(index.Value+2, addDoorRegisterBtn);

            return new string[] { dl.ID, addDoorRegisterBtn.ID, brk.ID };

        }

        
    }
}