using AdminWebPortal.Utils;
using System;
using System.Web.UI;

namespace AdminWebPortal.Views.Main.QRPages
{
    public partial class AdminUserQR : System.Web.UI.Page
    {
        private readonly string QRObject = "ViewState_QR";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                QRMethods QR = new QRMethods(Request.QueryString["key"], Request.QueryString["guid"], true);
                if (QR.GetRegistrationQR(ref qrImg, Request.QueryString["data"]))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "pollScript" + UniqueID, "pollTimeout(false);", true);
                    RegisterQRDiv.Visible = true;
                    ViewState[QRObject] = QR.SerializeObject(true);
                }
            }
 
        }

        protected void hiddenStatusCheckBtn_Click(object sender, EventArgs e)
        {
            QRMethods QR;
            if (((string)ViewState[QRObject]).TryDeserializeObject(out QR, true))
            {
                ScriptManager.RegisterStartupScript(hiddenPostBackUptPnl, hiddenPostBackUptPnl.GetType(), "statusScript" + UniqueID, QR.checkStatus(), true);
            }
        }

        protected void hiddenQRCompleteBtn_Click(object sender, EventArgs e)
        {
            qrImg.Visible = false;
        }
    }
}