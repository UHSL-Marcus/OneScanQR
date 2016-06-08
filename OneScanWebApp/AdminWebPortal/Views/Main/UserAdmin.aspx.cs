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

        protected List<int> OpenDoorTables
        {
            get
            {
                if (ViewState["OpenDoorTables"] == null)
                    ViewState["OpenDoorTables"] = new List<int>();

                return (List<int>)ViewState["OpenDoorTables"];
            }
            set { ViewState["OpenDoorTables"] = value; }
        }

        //TODO: Viewstate not saving!!
        protected override void OnInit(EventArgs e)
        {
            if (ViewState["tester"] != null)
            {
                string v = (string)ViewState["tester"];
            }

            List<int> l = OpenDoorTables;
            fillTable();
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            OpenDoorTables = new List<int>() { 2, 3 };
            ViewState["tester"] = "testing";
        }

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

        private class DoorViewBtnArguments
        {
            public int? RowIndex;
            public int? CellIndex;
            public int? UserTokenID;
            public bool showingDoors = false;
        }

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
                delBtn.CommandArgument = string.Join(",", new string[] { });

                tCell.Controls.Add(delBtn);

                if(OpenDoorTables.Contains(entry.UserTokenID.Value))
                    tCell.Controls.Add(buildDoorViewTable(entry.UserTokenID.Value.ToString()));
                

                tRow.Cells.Add(tCell);

                usersTbl.Rows.Add(tRow);

                doorViewBtnArgs.CellIndex = tRow.Cells.GetCellIndex(tCell);
                doorViewBtnArgs.RowIndex = usersTbl.Rows.GetRowIndex(tRow);

                showDoorViewBtn.CommandArgument = JsonUtils.GetJson(doorViewBtnArgs);
                
            }
        }

        private Table buildDoorViewTable(string userTknID)
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

            List<DoorViewInfo> info = SQLControls.getData<DoorViewInfo>(new DoorViewInfo(userTknID));

            foreach (DoorViewInfo entry in info)
            {
                TableRow tRow = new TableRow();

                TableCell tCell = new TableCell();
                tCell.Text = entry.DoorID;
                tRow.Cells.Add(tCell);

                tCell = new TableCell();

                Button removeDoorAuthBtn = new Button();
                removeDoorAuthBtn.Text = "Remove Authorisation";
                removeDoorAuthBtn.ID = "RemoveAuthorisation" + entry.Id;
                removeDoorAuthBtn.Click += RemoveDoorAuthBtn_Click;
                removeDoorAuthBtn.CommandArgument = string.Join(",", new string[] { });

                tCell.Controls.Add(removeDoorAuthBtn);


                tRow.Cells.Add(tCell);

                DoorTable.Rows.Add(tRow);
            }

            return DoorTable;
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

            public DoorViewInfo (string UserTokenID)
            {
                this.UserTokenID = UserTokenID;
            }

            public string getSQL()
            {
                return SelectSQL.Replace("@UserTokenID", UserTokenID);
            }
        } 

        private void DoorViewBtn_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                DoorViewBtnArguments args = JsonUtils.GetObject<DoorViewBtnArguments>(btn.CommandArgument);
                string userTknID = args.UserTokenID.ToString();

                TableCell recieveCell = usersTbl.Rows[args.RowIndex.Value].Cells[args.CellIndex.Value];
                

                if (args.showingDoors)
                {
                    OpenDoorTables.Remove(args.UserTokenID.Value);
                    btn.Text = "Show Registered Doors";
                    args.showingDoors = false;
                }
                else
                {

                    recieveCell.Controls.Add(buildDoorViewTable(userTknID));

                    OpenDoorTables.Add(args.UserTokenID.Value);
                    args.showingDoors = true;
                    btn.Text = "Hide Registered Doors";
                }

                btn.CommandArgument = JsonUtils.GetJson(args);

            }


        }

        private void RemoveDoorAuthBtn_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}