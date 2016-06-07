using AdminWebPortal.Database;
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
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public class TableInfo
        {
            public int? UserInfoID;
            public int? UserTokenID;
            public string UserInfoName;
            public string UserTokenToken;

            [SQLControls.SQLIgnore]
            public static string SelectSQL = "SELECT UserInfo.Id AS UserInfoID, UserToken.Id AS UserTokenID, UserInfo.Name AS UserInfoName, UserToken.Token AS UserTokenToken FROM UserInfo JOIN UserToken ON UserToken.Id=UserInfo.UserToken";  
        }

        private void fillTable()
        {
            TableRow headings = usersTbl.Rows[0];
            usersTbl.Rows.Clear();
            usersTbl.Rows.Add(headings);


            

            while (reader.Read())
            {
                TableRow tRow = new TableRow();
                TableCell tCell;

                for (int i = 0; i < 2; i++)
                {
                    tCell = new TableCell();
                    tCell.Text = reader.GetString(i);
                    tRow.Cells.Add(tCell);
                }

                tCell = new TableCell();

                Button doorViewBtn = new Button();
                doorViewBtn.Text = "Registered Doors";
                doorViewBtn.ID = "RegisteredDoors" + reader.GetInt32(2).ToString();
                doorViewBtn.Click += DoorViewBtn_Click;
                doorViewBtn.CommandArgument = string.Join(",", new string[] { });

                tCell.Controls.Add(doorViewBtn);

                Button delBtn = new Button();
                delBtn.Text = "Delete User";
                delBtn.ID = "deleteUser" + reader.GetInt32(2).ToString();
                delBtn.Click += DelBtn_Click;
                delBtn.CommandArgument = string.Join(",", new string[] { });

                tCell.Controls.Add(delBtn);
                tRow.Cells.Add(tCell);

                usersTbl.Rows.Add(tRow);
            }
        }

        private void DoorViewBtn_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}