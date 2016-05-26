using System;
using System.Windows.Forms;

using OneScanQR.PayloadObjects;
using OneScanQR.Utils;
using System.Drawing;

namespace OneScanQR
{
    public partial class QRGeneratorUI : Form
    {
        public QRGeneratorUI()
        {
            InitializeComponent();

            LoginActionCmbx.DataSource = Enum.GetValues(typeof(LoginTypes));
        }


        private void QRGenBtn_Click(object sender, EventArgs e)
        {
            //string data = "%7b%22ProcessType%22%3a%22Login%22%2c%22MessageType%22%3a%22StartLogin%22%2c%22SessionData%22%3a%22%7b%5c%22FullPayload%5c%22%3a%7b%5c%22ProcessType%5c%22%3a%5c%22Login%5c%22%2c%5c%22MessageType%5c%22%3a%5c%22StartLogin%5c%22%2c%5c%22SessionData%5c%22%3a%5c%22CUSTOM+SESSION+DATA%5c%22%2c%5c%22Version%5c%22%3a2%2c%5c%22MetaData%5c%22%3a%7b%5c%22EndpointURL%5c%22%3a%5c%22http%3a%2f%2fapiharness.ensygnia.net%2fOnescanCallback.aspx%5c%22%7d%2c%5c%22LoginPayload%5c%22%3a%7b%5c%22FriendlyName%5c%22%3a%5c%22API+Harness%5c%22%2c%5c%22SiteIdentifier%5c%22%3a%5c%22EE9D8995-04BB-452A-A4FE-066D3550662A%5c%22%2c%5c%22LoginMode%5c%22%3a%5c%22UserToken%5c%22%7d%7d%2c%5c%22TransactionId%5c%22%3a%5c%228082d60e-0fde-621e-69a4-ccc80df5d496%5c%22%2c%5c%22LoginResponseType%5c%22%3a%5c%22ProcessComplete%5c%22%7d%22%2c%22Version%22%3a2%2c%22MetaData%22%3a%7b%22EndpointURL%22%3a%22http%3a%2f%2fapiharness.ensygnia.net%2fOnescanCallback.aspx%22%7d%2c%22LoginPayload%22%3a%7b%22FriendlyName%22%3a%22API+Harness%22%2c%22SiteIdentifier%22%3a%22EE9D8995-04BB-452A-A4FE-066D3550662A%22%2c%22LoginMode%22%3a%22UserToken%22%7d%7d";

            //textBox1.Text = Uri.(data);
            LoginTypes lType;
            if (Enum.TryParse(LoginActionCmbx.SelectedValue.ToString(), out lType))
            {
                BasePayload payload = new BasePayload();
                payload.SetLoginPayload(lType);
                string loginJson = payload.GetJson(true);

                Bitmap QR;
                OneScanRequests oneScan = new OneScanRequests();
                if (oneScan.padlockStart(loginJson, out QR))
                {
                    QRBox.Image = QR;
                    string url;
                    if (oneScan.padlockContinue(out url))
                        urlTxtBx.Text = url;
                    else urlTxtBx.Text = "Continue Failed";
                }
                else urlTxtBx.Text = "Padlock Failed";
            }
        }
    }
}
