using AdminWebPortal.Database;
using AdminWebPortal.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AdminWebPortal.Views.Main.QRPages
{
    public partial class UserQR : System.Web.UI.Page
    {
        private readonly string GUID = "session_guid";
        private readonly string KEY = "session_key";
        private readonly string SECRET = "session_secret";
        protected void Page_Load(object sender, EventArgs e)
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
                if (HTTPRequest.HTTPGetRequest("http://localhost:3469/OneScanRequestSession.ashx?" + query, out reply))
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
        }
    }
}