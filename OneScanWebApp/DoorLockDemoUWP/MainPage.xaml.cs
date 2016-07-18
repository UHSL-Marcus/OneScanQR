using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Windows;
using System.Drawing;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Core;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DoorLockDemoUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private BackgroundWorker bgWorker;
        private AppStateMachine stateMachine = new AppStateMachine();
        private byte[] imgData = new byte[0];

        private int cancelBtnGetQrY;
        private int cancelBtnScanningY;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            stateMachineInit();

        }

        private void stateMachineInit()
        {
            stateMachine.setStateCallback(AppStateMachine.State.LOCKED, new Task (() => {
                setControlVisible(qr_img, Visibility.Collapsed);
                setControlVisible(getQR_btn, Visibility.Visible);
                setControlVisible(cancel_btn, Visibility.Collapsed);
                setControlVisible(reset_btn, Visibility.Collapsed);
                setControlVisible(radioLoad_img, Visibility.Collapsed);
                setLockImg("Assets/locked-padlock.png");
            }));
            stateMachine.setStateCallback(AppStateMachine.State.REQUESTING_QR, new Task(() => {
                setControlVisible(getQR_btn, Visibility.Collapsed);
                setControlVisible(cancel_btn, Visibility.Visible);
                setControlVisible(radioLoad_img, Visibility.Visible);
            }));
            stateMachine.setStateCallback(AppStateMachine.State.QR_DISPLAY, new Task(() => {
                setQRPicture();
                setControlVisible(qr_img, Visibility.Visible);
                setControlVisible(cancel_btn, Visibility.Collapsed);
                setControlVisible(radioLoad_img, Visibility.Collapsed); 
            }));
            stateMachine.setStateCallback(AppStateMachine.State.SCANNING, new Task(() => {
                setControlVisible(qr_img, Visibility.Collapsed);
                setControlVisible(cancel_btn, Visibility.Visible);
                setLockImg("Assets/loadingRoll.gif");
            }));
            stateMachine.setStateCallback(AppStateMachine.State.UNLOCKED, new Task(() => {
                setControlVisible(qr_img, Visibility.Collapsed);
                setControlVisible(cancel_btn, Visibility.Collapsed);
                setControlVisible(reset_btn, Visibility.Visible);
                setLockImg("Assets/unlocked-padlock.png");
            }));

            stateMachine.start();
        }

        private void cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            bgWorker.CancelAsync();
            stateMachine.doEvent(AppStateMachine.Event.CANCELLED);
        }

        private void getQR_btn_Click(object sender, RoutedEventArgs e)
        {
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(QRRequests.doGetQR);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_GetQRDone);
            bgWorker.WorkerSupportsCancellation = true;

            bgWorker.RunWorkerAsync();

            stateMachine.doEvent(AppStateMachine.Event.QR_REQUEST);
        }

        private void reset_btn_Click(object sender, RoutedEventArgs e)
        {
            stateMachine.doEvent(AppStateMachine.Event.RESET);
        }

        private void getQR_callback()
        {


            if (!e.Cancelled)
            {
                Tuple<bool, byte[]> result = e.Result as Tuple<bool, byte[]>;
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


        internal async void setQRPicture()
        {
            await qr_img.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                try
                {
                    var buffer = Convert.FromBase64String(System.Text.Encoding.UTF8.GetString(imgData));
                    var image = new BitmapImage();
                    using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
                    {
                        using (DataWriter writer = new DataWriter(ms.GetOutputStreamAt(0)))
                        {
                            writer.WriteBytes(buffer);
                            await writer.StoreAsync();
                        }

                        image.SetSource(ms);
                    }

                    qr_img.Source = image;

                }
                catch (Exception) { }

            });
            
        }

        internal async void setControlVisible(UIElement ctl, Visibility visible)
        {
            await ctl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ctl.Visibility = visible;
            });
        }

        
        internal async void setLockImg(string uri)
        {
            await padlock_Img.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => 
            {
                padlock_Img.Source = new BitmapImage(new Uri(uri, UriKind.RelativeOrAbsolute));
            });
        }


    }
}
