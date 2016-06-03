using OneScanWebApp.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OneScanWebApp.Admin
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string currDomain = Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host;
            if (Request.Url.Port != 80 && Request.Url.Port != 443)
                currDomain += (":" + Request.Url.Port);

            string guid = Guid.NewGuid().ToString();
            string query = "mode=0&qr_img=1&guid=" + guid;
            string hmac = HMAC.Hash(query, ConfigurationManager.AppSettings["AdminSecret"]);
            query += "&data=" + hmac;

            byte[] reply;
            HTTPRequest.HTTPGetRequest(currDomain + "/OneScanAdminRequestSession.ashx?" + query, out reply);

            qrImg.ImageUrl = "data:image/bmp;base64," + System.Text.Encoding.Default.GetString(reply);

            query = "mode=0&door_id=1";
            hmac = HMAC.Hash(query, ConfigurationManager.AppSettings["AdminSecret"]);
            query += "&data=" + hmac;

            Page.ClientScript.RegisterStartupScript(GetType(), "pollScript", "pollOneScan('" + currDomain + "/OneScanAdminGetResult.ashx?" + query + "')", true);
        }
    }
}