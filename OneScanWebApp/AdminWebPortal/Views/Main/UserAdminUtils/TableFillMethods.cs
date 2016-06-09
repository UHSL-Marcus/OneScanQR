using AdminWebPortal.Database;
using AdminWebPortal.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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


            List<TableInfo> info = SQLControls.getData<TableInfo>(new TableInfo());

            foreach (TableInfo entry in info)
            {
                TableRow tRow = new TableRow();

                TableCell tCell = new TableCell();
                tCell.Text = entry.UserInfoName;
                tRow.Cells.Add(tCell);

                tCell = new TableCell();
                tCell.Text = entry.UserTokenToken;
                tRow.Cells.Add(tCell);

                tCell = new TableCell();

                Button showDoorViewBtn = new Button();
                showDoorViewBtn.Text = "Show Registered Doors";
                showDoorViewBtn.ID = "ShowRegisteredDoors" + entry.UserTokenID;
                showDoorViewBtn.Click += DoorViewBtn_Click;
                DoorViewBtnArguments doorViewBtnArgs = new DoorViewBtnArguments();
                doorViewBtnArgs.UserTokenID = entry.UserTokenID;

                tCell.Controls.Add(showDoorViewBtn);


                Button delBtn = new Button();
                delBtn.Text = "Delete User";
                delBtn.ID = "deleteUser" + entry.UserTokenID;
                delBtn.Click += DelBtn_Click;
                DeleteBtnArguments deletebtnArgs = new DeleteBtnArguments();
                deletebtnArgs.UserInfoID = entry.UserInfoID;
                deletebtnArgs.UserTokenID = entry.UserTokenID;

                tCell.Controls.Add(delBtn);

                if (OpenDoorTables.Contains(entry.UserTokenID.Value))
                    tCell.Controls.Add(buildDoorViewTable(entry.UserTokenID.Value));


                tRow.Cells.Add(tCell);

                usersTbl.Rows.Add(tRow);

                doorViewBtnArgs.CellIndex = tRow.Cells.GetCellIndex(tCell);
                doorViewBtnArgs.RowIndex = usersTbl.Rows.GetRowIndex(tRow);

                deletebtnArgs.RowIndex = usersTbl.Rows.GetRowIndex(tRow);

                showDoorViewBtn.CommandArgument = JsonUtils.GetJson(doorViewBtnArgs);
                delBtn.CommandArgument = JsonUtils.GetJson(deletebtnArgs);

            }
        }

        private Table buildDoorViewTable(int userTknID)
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

            List<DoorViewInfo> info = SQLControls.getData<DoorViewInfo>(new DoorViewInfo(userTknID.ToString()));

            foreach (DoorViewInfo entry in info)
            {
                TableRow tRow = new TableRow();
                tRow.ID = "DoorViewTableRow" + entry.DoorID;

                TableCell tCell = new TableCell();
                tCell.Text = entry.DoorID;
                tRow.Cells.Add(tCell);

                tCell = new TableCell();

                Button removeDoorAuthBtn = new Button();
                removeDoorAuthBtn.Text = "Remove Authorisation";
                removeDoorAuthBtn.ID = "RemoveAuthorisation" + entry.Id;
                removeDoorAuthBtn.Click += RemoveDoorAuthBtn_Click;

                RemoveAuthBtnArguments removeAuthArgs = new RemoveAuthBtnArguments();
                removeAuthArgs.UserTokenID = userTknID;
                removeAuthArgs.DoorTableID = DoorTable.ID;
                removeAuthArgs.DoorTableRowID = tRow.ID;

                removeDoorAuthBtn.CommandArgument = JsonUtils.GetJson(removeAuthArgs);

                tCell.Controls.Add(removeDoorAuthBtn);


                tRow.Cells.Add(tCell);

                DoorTable.Rows.Add(tRow);
            }

            return DoorTable;
        }
    }
}