using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace SignalRClient_test
{
    public partial class Form1 : Form
    {
        HubConnection hubConnection;
        IHubProxy chatProxy;
        public Form1()
        {
            InitializeComponent();

            hubConnection = new HubConnection("http://localhost/");
            chatProxy = hubConnection.CreateHubProxy("ChatHub");
            ServicePointManager.DefaultConnectionLimit = 10;
            hubConnection.ConnectionSlow += () => printMessage("Connection problems.");

            chatProxy.On<dynamic>("broadcastMessage", recieveMessage);

            hubConnection.Start().Wait();

            //chatProxy.On("broadcastMessage");
        }

        private void recieveMessage(dynamic m)
        {
            printMessage(m.name.Value + " : " + m.message.Value);
        }

        private delegate void SetTextCallback(string m);
        private void printMessage(string m)
        {
            if (recieveTxtBx.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(printMessage);
                Invoke(d, new object[] { m });
            }
            else recieveTxtBx.Text += "\n" + m;

        }

        private void SendBtn_Click(object sender, EventArgs e)
        {
            chatProxy.Invoke("Send", sendTxtbx.Text);
        }
    }
}
