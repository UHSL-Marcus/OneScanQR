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
        private readonly string GUID = "session_guid";
        private readonly string KEY = "session_key";
        private readonly string SECRET = "session_secret";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ScriptManager.GetCurrent(Page).IsInAsyncPostBack)
            {
                RegisterQRDiv.Visible = false;
                fillTable();
            }
        }

        private void fillTable()
        {
            TableRow headings = AdminUsersTbl.Rows[0];
            AdminUsersTbl.Rows.Clear();
            AdminUsersTbl.Rows.Add(headings);
            

            DataTableReader reader = SQLControls.getDataReader("SELECT AdminUser.Id, AdminToken.Id, AdminUser.Name, AdminToken.UserToken FROM AdminUser JOIN AdminToken ON AdminToken.Id=AdminUser.AdminToken");

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
            RegisterQRDiv.Visible = true;
            qrImg.ImageUrl = "";

            Session[KEY] = Guid.NewGuid().ToString();
            Session[SECRET] = Guid.NewGuid().ToString();
            if (SQLControls.doNonQuery("INSERT INTO RegistrationToken VALUES('" + Session[KEY] + "', '" + Session[SECRET] + "')"))
            {
                Session[GUID] = Guid.NewGuid().ToString();
                string query = "mode=1&qr_img=1&guid=" + Session[GUID] + "&key=" + Session[KEY];
                string hmac = HMAC.Hash(query, (string)Session[SECRET]);
                query += "&data=" + hmac;

                byte[] reply;
                if (HTTPRequest.HTTPGetRequest("http://localhost:3469/OneScanAdminRequestSession.ashx?" + query, out reply))
                {
                    qrImg.ImageUrl = "data:image/bmp;base64," + System.Text.Encoding.Default.GetString(reply);

                    ScriptManager.RegisterStartupScript(this, GetType(), "pollScript" + UniqueID, "pollTimeout();", true);
                }
            }
        }

        private string getPollUrl()
        {
            string query = "mode=1&guid=" + Session[GUID] + "&key=" + Session[KEY];
            string hmac = HMAC.Hash(query, (string)Session[SECRET]);
            query += "&data=" + hmac;

            return "http://localhost:3469/OneScanAdminGetResult.ashx?" + query;
        }

        protected void hiddenStatusCheckBtn_Click(object sender, EventArgs e)
        {
            byte[] reply;
            if (HTTPRequest.HTTPGetRequest(getPollUrl(), out reply))
            {
                int status;
                if (int.TryParse(System.Text.Encoding.Default.GetString(reply), out status))
                {
                    if (status < 2)
                    {
                        ScriptManager.RegisterStartupScript(hiddenPostBackUptPnl, hiddenPostBackUptPnl.GetType(), "pollScript" + UniqueID, "pollTimeout();", true);
                        if (status == 1)
                        {
                            // scanning
                        }
                    } 
                    else if (status >= 2)
                        ScriptManager.RegisterStartupScript(hiddenPostBackUptPnl, hiddenPostBackUptPnl.GetType(), "registrationFinishScript" + UniqueID, "RegistrationFinish();", true);

                }
            }
        }

        protected void hiddenQRCompleteBtn_Click(object sender, EventArgs e)
        {
            RegisterQRDiv.Visible = false;
            fillTable();
        }
    }
}