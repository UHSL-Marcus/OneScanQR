using AdminWebPortal.Utils;
using System;
using System.Collections.Generic;
using System.Data;
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

            List<TableInfo> info;
            if (SQLControlsLib.Get.doJoinSelect(new TableInfo(), out info))
            {
                foreach (TableInfo entry in info)
                {
                    TableRow tRow = new TableRow();
                    TableCell tCell;

                    tRow.Cells.AddTextCell(entry.AdminUser_Name);
                    tRow.Cells.AddTextCell(entry.AdminToken_UserToken);
                    
                    tCell = new TableCell();

                    Button delBtn = new Button();
                    delBtn.Text = "Delete User";
                    delBtn.ID = "deleteUser" + entry.AdminUserId.ToString();
                    delBtn.Click += DelBtn_Click;
                    delBtn.CommandArgument = string.Join(",", new string[] { entry.AdminUserId.ToString(), entry.AdminTokenId.ToString() });

                    tCell.Controls.Add(delBtn);
                    tRow.Cells.Add(tCell);

                    AdminUsersTbl.Rows.Add(tRow);
                }
            }
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            if(sender is Button)
            {
                Button btn = (Button)sender;
                //DeleteButtonParameters args = JsonUtils.GetObject<DeleteButtonParameters>(btn.CommandArgument);
                string[] args = btn.CommandArgument.Split(',');

                SQLControlsLib.Delete.doDeleteEntryByColumn("AdminUser", int.Parse(args[0]), "Id");
                SQLControlsLib.Delete.doDeleteEntryByColumn("AdminToken", int.Parse(args[1]),"Id");

                fillTable();
            }
        }

        protected void registerNewAdminBtn_Click(object sender, EventArgs e)
        {
            string key = Guid.NewGuid().ToString();
            string secret = Guid.NewGuid().ToString();

            AdminWebApp.Database.Objects.RegistrationToken rt = new AdminWebApp.Database.Objects.RegistrationToken();
            rt.AuthKey = key; rt.Secret = secret;
            if (SQLControlsLib.Set.doInsert(rt))
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