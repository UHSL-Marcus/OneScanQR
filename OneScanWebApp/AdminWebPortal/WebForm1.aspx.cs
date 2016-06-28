using AdminWebPortal.Utils;
using HTTPRequestLib;
using System;
using System.Text;

namespace AdminWebPortal
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            byte[] reply;
            if (Get.HTTPGetRequest("https://mmtsnap.mmt.herts.ac.uk/onescan/errortest.ashx", out reply))
            {
                response.Text = Encoding.Default.GetString(reply);
            }
        }
    }
}