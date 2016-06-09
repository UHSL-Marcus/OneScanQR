using AdminWebPortal.Database;
using AdminWebPortal.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AdminWebPortal.Views.Main
{
    public partial class UserAdmin : System.Web.UI.Page
    {

        protected List<int> OpenDoorTables = new List<int>();

        protected override void OnInit(EventArgs e)
        {
            Request.Form[doorViewState.UniqueID].TryDeserializeObject(out OpenDoorTables, true);

            fillTable();
        }

        protected override void OnPreRender(EventArgs e)
        {
           doorViewState.Value = OpenDoorTables.SerializeObject(true);
        }

        private void DoorViewBtn_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                DoorViewBtnArguments args = JsonUtils.GetObject<DoorViewBtnArguments>(btn.CommandArgument);
                int userTknID = args.UserTokenID.Value;

                TableCell recieveCell = usersTbl.Rows[args.RowIndex.Value].Cells[args.CellIndex.Value];
                

                if (args.showingDoors)
                {
                    OpenDoorTables.Remove(userTknID);
                    recieveCell.Controls.RemoveAt(args.DoorTableIndex.Value);
                    btn.Text = "Show Registered Doors";
                    args.showingDoors = false;
                    args.DoorTableIndex = null;
                }
                else
                {

                    Table doorViewTable = buildDoorViewTable(userTknID);
                    recieveCell.Controls.Add(doorViewTable);

                    args.DoorTableIndex = recieveCell.Controls.IndexOf(doorViewTable);

                    OpenDoorTables.Add(args.UserTokenID.Value);
                    args.showingDoors = true;
                    btn.Text = "Hide Registered Doors";
                }

                btn.CommandArgument = JsonUtils.GetJson(args);

            }


        }

        private void RemoveDoorAuthBtn_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                RemoveAuthBtnArguments args = JsonUtils.GetObject<RemoveAuthBtnArguments>(btn.CommandArgument);

                Control DoorTableCtl = usersTbl.FindControlRecursive(args.DoorTableID);
                if (DoorTableCtl is Table)
                {
                    Table DoorTable = (Table)DoorTableCtl;
                    Control RowToRemove = DoorTable.FindControlRecursive(args.DoorTableRowID);

                    if (RowToRemove is TableRow)
                    {
                        string queryUTP = "DELETE FROM DoorUserTokenPair WHERE UserToken='" + args.UserTokenID.Value + "'";
                        SQLControls.doNonQuery(queryUTP);

                        DoorTable.Rows.Remove((TableRow)RowToRemove);
                    }
                }
                  
            }
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                DeleteBtnArguments args = JsonUtils.GetObject<DeleteBtnArguments>(btn.CommandArgument);

                string queryUTP = "DELETE FROM DoorUserTokenPair WHERE UserToken='" + args.UserTokenID.Value + "'";
                string queryU = "DELETE FROM UserInfo WHERE Id='" + args.UserInfoID.Value + "'";
                string queryT = "DELETE FROM UserToken WHERE Id='" + args.UserTokenID.Value + "'";

                SQLControls.doNonQuery(queryUTP);
                SQLControls.doNonQuery(queryU);
                SQLControls.doNonQuery(queryT);

                usersTbl.Rows.RemoveAt(args.RowIndex.Value);

            }
        }

        
    }
}