using System;
using System.Drawing;
using System.Windows.Forms;
using HTTPRequestLib;
using CryptoLib;
using System.Diagnostics;
using System.IO;
using System.ComponentModel;
using System.Threading;

namespace DoorLockDemo
{
    public partial class Form1 : Form
    {

        private BackgroundWorker bgWorker;
        private AppStateMachine stateMachine = new AppStateMachine();
        private byte[] imgData = new byte[0];

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            getQrBtn.Parent = btnPnl;
            int x = (btnPnl.Width/2) - (getQrBtn.Width/2);
            int y = (btnPnl.Height/2) - (getQrBtn.Height/2);
            getQrBtn.Location = new Point(x, y);

            cancelBtn.Parent = btnPnl;
            x = (btnPnl.Width / 2) - (cancelBtn.Width / 2);
            y = (btnPnl.Height / 2) - (cancelBtn.Height / 2);
            cancelBtn.Location = new Point(x, y);

            resetBtn.Parent = btnPnl;
            x = (btnPnl.Width / 2) - (resetBtn.Width / 2);
            y = (btnPnl.Height / 2) - (resetBtn.Height / 2);
            resetBtn.Location = new Point(x, y);

            stateMachineInit();
        }

        private void stateMachineInit()
        {
            stateMachine.setStateCallback(AppStateMachine.State.LOCKED, delegate () {
                setQRPicture(false);
                setQRBtnVisible(true);
                setCancelBtnVisible(false);
                setResetBtnVisible(false);
                setLockImg(Properties.Resources.locked_padlock);
            });
            stateMachine.setStateCallback(AppStateMachine.State.REQUESTING_QR, delegate () {
                setQRBtnVisible(false);
                setCancelBtnVisible(true);
                // add loading
            });
            stateMachine.setStateCallback(AppStateMachine.State.QR_DISPLAY, delegate () {
                setQRPicture(true);
                setCancelBtnVisible(false);
                setQRBtnVisible(false);
            });
            stateMachine.setStateCallback(AppStateMachine.State.SCANNING, delegate () {
                setQRPicture(false);
                setCancelBtnVisible(true);
                setLockImg(Properties.Resources.loadingRoll);
            });
            stateMachine.setStateCallback(AppStateMachine.State.UNLOCKED, delegate () {
                setQRPicture(false);
                setCancelBtnVisible(false);
                setResetBtnVisible(true);
                setLockImg(Properties.Resources.unlocked_padlock);
            });

            stateMachine.start();
        }


        private void getQrBtn_Click(object sender, EventArgs e)
        {

            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(QRRequests.doGetQR);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_GetQRDone);
            bgWorker.WorkerSupportsCancellation = true;

            bgWorker.RunWorkerAsync();

            stateMachine.doEvent(AppStateMachine.Event.QR_REQUEST);
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            bgWorker.CancelAsync();
            stateMachine.doEvent(AppStateMachine.Event.CANCELLED);
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            stateMachine.doEvent(AppStateMachine.Event.RESET);
        }

        private void backgroundWorker_GetQRDone(object sender, RunWorkerCompletedEventArgs e)
        {
            Tuple<bool, byte[]> result = e.Result as Tuple<bool, byte[]>;
            
            if (!e.Cancelled)
                if (result.Item1)
                {
                    imgData = result.Item2;

                    bgWorker = new BackgroundWorker();
                    bgWorker.DoWork += new DoWorkEventHandler(QRRequests.doGetResult);
                    bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_GetStatusDone);
                    bgWorker.WorkerReportsProgress = true;
                    bgWorker.WorkerSupportsCancellation = true;
                    bgWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_GetStatusProgress);

                    bgWorker.RunWorkerAsync();

                    stateMachine.doEvent(AppStateMachine.Event.GOT_QR);
                } 
        }

        private void backgroundWorker_GetStatusDone(object sender, RunWorkerCompletedEventArgs e)
        {

            if (!e.Cancelled)
            {
                int? result = e.Result as int?;

                if (result == 2)
                    stateMachine.doEvent(AppStateMachine.Event.SCAN_ACCEPTED);
                else if (result == 3)
                    stateMachine.doEvent(AppStateMachine.Event.SCAN_DENIED);
            }
        }

        private void backgroundWorker_GetStatusProgress(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
                stateMachine.doEvent(AppStateMachine.Event.QR_SCANNED);
        }

        public delegate void setQRPictureDel(bool visible);
        internal void setQRPicture(bool visible)
        {
            if (qrPictBx.InvokeRequired)
                qrPictBx.BeginInvoke(new setQRPictureDel(setQRPicture), new object[] { visible });
            else {
                try
                {
                    byte[] byteBuffer = Convert.FromBase64String(System.Text.Encoding.Default.GetString(imgData));
                    using (MemoryStream ms = new MemoryStream(byteBuffer))
                    {
                        Bitmap bmp = new Bitmap(ms);
                        qrPictBx.Image = bmp;
                    }
                }
                catch (Exception) { }

                qrPictBx.Visible = visible;
                
            }
        }

        public delegate void setQRBtnVisibleDel(bool visible);
        internal void setQRBtnVisible(bool visible)
        {
            if (getQrBtn.InvokeRequired)
                getQrBtn.BeginInvoke(new setQRBtnVisibleDel(setQRBtnVisible), new object[] { visible });
            else
                getQrBtn.Visible = visible;
        }

        public delegate void setCancelBtnVisibleDel(bool visible);
        internal void setCancelBtnVisible(bool visible)
        {
            if (cancelBtn.InvokeRequired)
                cancelBtn.BeginInvoke(new setCancelBtnVisibleDel(setCancelBtnVisible), new object[] { visible });
            else
                cancelBtn.Visible = visible;
        }

        public delegate void setResetBtnVisibleDel(bool visible);
        internal void setResetBtnVisible(bool visible)
        {
            if (resetBtn.InvokeRequired)
                resetBtn.BeginInvoke(new setResetBtnVisibleDel(setResetBtnVisible), new object[] { visible });
            else
                resetBtn.Visible = visible;
        }

        public delegate void setLockImgDel(Bitmap img);
        internal void setLockImg(Bitmap img)
        {
            if (padlockPictBx.InvokeRequired)
                padlockPictBx.BeginInvoke(new setLockImgDel(setLockImg), new object[] { img });
            else {
                padlockPictBx.Image = img;
            }
        }

        
    }
}
