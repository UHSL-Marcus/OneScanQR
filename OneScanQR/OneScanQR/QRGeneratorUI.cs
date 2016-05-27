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
            urlTxtBx.Text = "";

            LoginTypes lType;
            if (Enum.TryParse(LoginActionCmbx.SelectedValue.ToString(), out lType))
            {
                
                Bitmap QR;
                OneScanRequests oneScan = new OneScanRequests();
                if (oneScan.padlockStart(PayloadFactory.GetPayload(lType), out QR))
                {
                    QRBox.Visible = true;
                    responseBrowser.Visible = false;
                    QRBox.Image = QR;
                    
                    if (!oneScan.padlockContinue(ScanComplete))
                        urlTxtBx.Text = "Continue Failed";
                }
                else urlTxtBx.Text = "Padlock Failed";
            }
        }

        private delegate void SetTextCallback(bool succcess, string url);
        private void ScanComplete(bool succcess, string url)
        {
            if (urlTxtBx.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(ScanComplete);
                Invoke(d, new object[] { succcess, url });
            }
            else
            {
                if (succcess)
                {
                    urlTxtBx.Text = url;
                    QRBox.Visible = false;
                    responseBrowser.Visible = true;
                    responseBrowser.Navigate("http://apiharness.ensygnia.net" + url);
                }
                else 
                    urlTxtBx.Text = "Continue Failed callback";
            }
        }
    }
}
