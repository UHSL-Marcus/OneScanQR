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
        private string guid;
        private string key;
        private string secret;
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterQRDiv.Visible = false;
            fillTable();
        }

        private void fillTable()
        {
            DataTableReader reader = SQLControls.getDataReader("SELECT AdminUser.Id, AdminUser.Name, AdminToken.UserToken FROM AdminUser JOIN AdminToken ON AdminToken.Id=AdminUser.AdminToken");

            while (reader.Read())
            {
                TableRow tRow = new TableRow();
                TableCell tCell;

                for (int i = 1; i < 3; i++)
                {
                    tCell = new TableCell();
                    tCell.Text = reader.GetString(i);
                    tRow.Cells.Add(tCell);
                }

                tCell = new TableCell();
                tCell.Text = "Delete User: " + reader.GetInt32(0);
                tRow.Cells.Add(tCell);

                AdminUsersTbl.Rows.Add(tRow);
            }
        }

        protected void registerNewAdminBtn_Click(object sender, EventArgs e)
        {
            RegisterQRDiv.Visible = true;
            qrImg.ImageUrl = "";

            key = Guid.NewGuid().ToString();
            secret = Guid.NewGuid().ToString();
            if (SQLControls.doNonQuery("INSERT INTO RegistrationToken VALUES('" + key + "', '" + secret + "')"))
            {
                guid = Guid.NewGuid().ToString();
                string query = "mode=1&qr_img=1&guid=" + guid + "&key=" + key;
                string hmac = HMAC.Hash(query, secret);
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
            string query = "mode=1&guid=" + guid + "&key=" + key;
            string hmac = HMAC.Hash(query, secret);
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
                        ScriptManager.RegisterStartupScript(hiddenPostBackUptPnl, hiddenPostBackUptPnl.GetType(), "pollScript" + UniqueID, "pollTimeout();", true);
                    else if (status > 2)
                        ScriptManager.RegisterStartupScript(hiddenPostBackUptPnl, hiddenPostBackUptPnl.GetType(), "registrationFinishScript" + UniqueID, "RegistrationFinish();", true);

                }
            }
        }

        protected void hiddenQRCompleteBtn_Click(object sender, EventArgs e)
        {

        }
    }
}