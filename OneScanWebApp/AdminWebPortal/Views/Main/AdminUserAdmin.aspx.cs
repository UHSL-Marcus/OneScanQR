using AdminWebPortal.Database;
using AdminWebPortal.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AdminWebPortal.Views.Main
{
    public partial class AdminUserAdmin : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ScriptManager.GetCurrent(Page).IsInAsyncPostBack)
            {
                fillTable();
            }
        }

        private void fillTable()
        {
            TableRow headings = AdminUsersTbl.Rows[0];
            AdminUsersTbl.Rows.Clear();
            AdminUsersTbl.Rows.Add(headings);
            

            DataTableReader reader = SQLControls.Get.getDataReader("SELECT AdminUser.Id, AdminToken.Id, AdminUser.Name, AdminToken.UserToken FROM AdminUser JOIN AdminToken ON AdminToken.Id=AdminUser.AdminToken");

            while (reader.Read())
            {
                TableRow tRow = new TableRow();
                TableCell tCell;

                for (int i = 2; i < 4; i++)
                {
                    tCell = new TableCell();
                    tCell.Text = reader.GetString(i);
                    tRow.Cells.Add(tCell);
                }

                tCell = new TableCell();

                Button delBtn = new Button();
                delBtn.Text = "Delete User";
                delBtn.ID = "deleteUser" + reader.GetInt32(0).ToString();
                delBtn.Click += DelBtn_Click;
                delBtn.CommandArgument = string.Join(",", new string[] { reader.GetInt32(0).ToString(), reader.GetInt32(1).ToString() });

                tCell.Controls.Add(delBtn);
                tRow.Cells.Add(tCell);

                AdminUsersTbl.Rows.Add(tRow);
            }
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            if(sender is Button)
            {
                Button btn = (Button)sender;
                //DeleteButtonParameters args = JsonUtils.GetObject<DeleteButtonParameters>(btn.CommandArgument);
                string[] args = btn.CommandArgument.Split(',');
                string queryU = "DELETE FROM AdminUser WHERE Id='" + args[0] + "'";
                string queryT = "DELETE FROM AdminToken WHERE Id='" + args[1] + "'";

                SQLControls.doNonQuery(queryU);
                SQLControls.doNonQuery(queryT);

                fillTable();
            }
        }

        protected void registerNewAdminBtn_Click(object sender, EventArgs e)
        {
            string key = Guid.NewGuid().ToString();
            string secret = Guid.NewGuid().ToString();
            if (SQLControls.doNonQuery("INSERT INTO RegistrationToken VALUES('" + key + "', '" + secret + "')"))
            {
                string guid = Guid.NewGuid().ToString();
                string query = "guid=" + guid + "&key=" + key;
                string hmac = HMAC.Hash(query, secret);
                query += "&data=" + hmac;

                Response.Redirect("~/Views/QRPages/AdminUserQR?" + query);
            }

            
        }


    }
}