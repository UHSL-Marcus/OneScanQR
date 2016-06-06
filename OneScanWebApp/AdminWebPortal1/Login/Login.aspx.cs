using OneScanWebApp.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;

namespace AdminWebpPortal
{
    public partial class Login : System.Web.UI.Page
    {

        private string guid;
        protected void Page_Load(object sender, EventArgs e)
        {
            getQR();
        }

        private void getQR()
        {
            guid = Guid.NewGuid().ToString();
            string query = "mode=0&qr_img=1&guid=" + guid;
            string hmac = HMAC.Hash(query, ConfigurationManager.AppSettings["AdminSecret"]);
            query += "&data=" + hmac;

            byte[] reply;
            HTTPRequest.HTTPGetRequest("http://localhost:3469/OneScanAdminRequestSession.ashx?" + query, out reply);

            qrImg.ImageUrl = "data:image/bmp;base64," + System.Text.Encoding.Default.GetString(reply);

            Page.ClientScript.RegisterStartupScript(GetType(), "pollScript", "pollOneScan('" + getPollUrl() + "')", true);
        }

        private string getPollUrl()
        {
            string query = "mode=0&guid=" + guid;
            string hmac = HMAC.Hash(query, ConfigurationManager.AppSettings["AdminSecret"]);
            query += "&data=" + hmac;

            return "http://localhost:3469/OneScanAdminGetResult.ashx?" + query;
        }

        protected void hiddenPostBackBtn_Click(object sender, EventArgs e)
        {
            FormsAuthentication.RedirectFromLoginPage("", false);
            /*byte[] reply;
            HTTPRequest.HTTPGetRequest(getPollUrl(), out reply);
            int status;
            if(int.TryParse(System.Text.Encoding.Default.GetString(reply), out status))
            {
                if (status == 2)
                    FormsAuthentication.RedirectFromLoginPage("", false);
            }*/
            
        }

        protected void hiddenNewQRBtn_Click(object sender, EventArgs e)
        {
            getQR();
        }
    }
}