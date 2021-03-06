﻿using AdminWebPortal.Utils;
using HTTPRequestLib;
using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace AdminWebPortal.Views.Login
{
    public partial class Login1 : System.Web.UI.Page
    {
        private readonly string GUID = "session_guid";
        protected void Page_Load(object sender, EventArgs e)
        {
            bool loggedIn = (HttpContext.Current.User != null) && HttpContext.Current.User.Identity.IsAuthenticated;
            if (loggedIn)
                Server.Transfer(FormsAuthentication.DefaultUrl);

            if (!IsPostBack)
                getQR();
        }

        private void getQR()
        {
            Session[GUID] = Guid.NewGuid().ToString();
            string query = "mode=0&qr_img=2&guid=" + Session[GUID];
            string hmac = HMAC.Hash(query, ConfigurationManager.AppSettings["AdminSecret"]);
            query += "&data=" + hmac;

            byte[] reply;
            Get.HTTPGetRequest(Consts.URL_BASE + "OneScanAdminRequestSession.ashx?" + query, out reply);

            qrImg.ImageUrl = "data:image/bmp;base64," + System.Text.Encoding.Default.GetString(reply);

            ScriptManager.RegisterStartupScript(this, GetType(), "pollScript" + UniqueID, "pollTimeout();", true);
        }

        private string getPollUrl()
        {
            string query = "mode=0&guid=" + Session[GUID];
            string hmac = HMAC.Hash(query, ConfigurationManager.AppSettings["AdminSecret"]);
            query += "&data=" + hmac;

            return Consts.URL_BASE + "OneScanAdminGetResult.ashx?" + query;
        }

        protected void hiddenNewQRBtn_Click(object sender, EventArgs e)
        {
            getQR();
        }

        protected void hiddenStatusCheckBtn_Click(object sender, EventArgs e)
        {
            byte[] reply;
            if (Get.HTTPGetRequest(getPollUrl(), out reply))
            {

                int status;
                if (int.TryParse(System.Text.Encoding.Default.GetString(reply), out status))
                {
                    string returnS = "";
                    string replace = "false";
                    if (status < 2)
                    {
                        if (status == 1) replace = "true";
                        returnS += ("pollTimeout(@scanned);").Replace("@scanned", replace);
                    }
                    else if (status == 2)
                        FormsAuthentication.RedirectFromLoginPage("", false);
                    else if (status == 3)
                        returnS += "ScanFailed();";
                    

                    ScriptManager.RegisterStartupScript(hiddenPostBackUptPnl, hiddenPostBackUptPnl.GetType(), "scanFailedScript" + UniqueID, returnS, true);

                }
            }
        }
    }
}