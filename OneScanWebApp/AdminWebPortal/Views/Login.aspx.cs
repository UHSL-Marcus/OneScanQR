using AdminWebPortal.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AdminWebPortal.Views.Login
{
    public partial class Login1 : System.Web.UI.Page
    {
        private readonly string GUID = "session_guid";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                getQR();
        }

        private void getQR()
        {
            Session[GUID] = Guid.NewGuid().ToString();
            string query = "mode=0&qr_img=1&guid=" + Session[GUID];
            string hmac = HMAC.Hash(query, ConfigurationManager.AppSettings["AdminSecret"]);
            query += "&data=" + hmac;

            byte[] reply;
            HTTPRequest.HTTPGetRequest("http://localhost:3469/OneScanAdminRequestSession.ashx?" + query, out reply);

            qrImg.ImageUrl = "data:image/bmp;base64," + System.Text.Encoding.Default.GetString(reply);

            ScriptManager.RegisterStartupScript(this, GetType(), "pollScript" + UniqueID, "pollTimeout();", true);
        }

        private string getPollUrl()
        {
            string query = "mode=0&guid=" + Session[GUID];
            string hmac = HMAC.Hash(query, ConfigurationManager.AppSettings["AdminSecret"]);
            query += "&data=" + hmac;

            return "http://localhost:3469/OneScanAdminGetResult.ashx?" + query;
        }

        protected void hiddenNewQRBtn_Click(object sender, EventArgs e)
        {
            getQR();
        }

        protected void hiddenStatusCheckBtn_Click(object sender, EventArgs e)
        {
            byte[] reply;
            if (HTTPRequest.HTTPGetRequest(getPollUrl(), out reply))
            {
                FormsAuthentication.RedirectFromLoginPage("", false);

                /*int status;
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
                    else if (status == 2)
                        FormsAuthentication.RedirectFromLoginPage("", false);
                    else if (status == 3)
                        ScriptManager.RegisterStartupScript(hiddenPostBackUptPnl, hiddenPostBackUptPnl.GetType(), "scanFailedScript" + UniqueID, "ScanFailed();", true);

                }*/
            }
        }
    }
}