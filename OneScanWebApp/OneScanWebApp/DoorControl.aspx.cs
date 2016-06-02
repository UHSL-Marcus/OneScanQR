﻿using OneScanWebApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OneScanWebApp
{
    public partial class DoorControl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void getQrbtn_Click(object sender, EventArgs e)
        {
            string currDomain = Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host;
            if (Request.Url.Port != 80 && Request.Url.Port != 443)
                currDomain += (":" + Request.Url.Port);

            currDomain += Request.ApplicationPath;

            string query = "mode=0&qr_img=1&door_id=1";
            string hmac = HMAC.Hash(query, "sec");
            query += "&data=" + hmac;

            byte[] reply;
            HTTPRequest.HTTPGetRequest(currDomain + "OneScanRequestSession.ashx?" + query, out reply);

            RequestResponse rr = JsonUtils.GetObject<RequestResponse>(System.Text.Encoding.Default.GetString(reply));

            qrImg.ImageUrl = "data:image/png;base64," + rr.Data;

            Page.ClientScript.RegisterStartupScript(GetType(), "pollScript", "pollOneScan('"+ rr.SessionID + "')", true);

        }
    }
}