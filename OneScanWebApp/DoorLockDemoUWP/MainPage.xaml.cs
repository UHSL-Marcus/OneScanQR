using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Core;
using System.Threading.Tasks;
using System.Threading;
using HTTPRequestLibUWP;
using XamlAnimatedGif;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DoorLockDemoUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private CancellationTokenSource cancelTknSrc;
        private AppStateMachine stateMachine = new AppStateMachine();
        private byte[] imgData = new byte[0];

        public MainPage()
        {

            InitializeComponent();
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            stateMachineInit();

            title_text.Text = "Door " + Consts.DoorID;
            
            await App.loadingTask.ContinueWith(new Action<Task> ((t) => 
            {
                stateMachine.doEvent(AppStateMachine.Event.LOADED);
            }));
        }

        private void stateMachineInit()
        {
            stateMachine.setStateCallback(AppStateMachine.State.LOADING, new Action(() => {
                setControlVisible(qr_img, Visibility.Collapsed);
                setControlVisible(getQR_btn, Visibility.Visible);
                setControlEnabled(getQR_btn, false);
                setControlVisible(cancel_btn, Visibility.Collapsed);
                setControlVisible(cancelling_btn, Visibility.Collapsed);
                setControlVisible(reset_btn, Visibility.Collapsed);
                setControlVisible(radioLoad_img, Visibility.Visible);
                setImg(radioLoad_img, "ms-appx:///Assets/Resources/LoadingEllipsis.gif", true);
                setImg(padlock_Img, "ms-appx:///Assets/Resources/locked-padlock.png");
            }));

            stateMachine.setStateCallback(AppStateMachine.State.LOCKED, new Action(() => {
                setControlVisible(qr_img, Visibility.Collapsed);
                setControlVisible(getQR_btn, Visibility.Visible);
                setControlEnabled(getQR_btn, true);
                setControlVisible(cancel_btn, Visibility.Collapsed);
                setControlVisible(cancelling_btn, Visibility.Collapsed);
                setControlVisible(reset_btn, Visibility.Collapsed);
                setControlVisible(radioLoad_img, Visibility.Collapsed);
                setImg(padlock_Img, "ms-appx:///Assets/Resources/locked-padlock.png");
            }));
            stateMachine.setStateCallback(AppStateMachine.State.REQUESTING_QR, new Action(() => {
                setControlVisible(getQR_btn, Visibility.Collapsed);
                setControlVisible(radioLoad_img, Visibility.Visible);
                setControlVisible(cancel_btn, Visibility.Visible);
                setImg(radioLoad_img, "ms-appx:///Assets/Resources/LoadingRadio.gif", true);
            }));
            stateMachine.setStateCallback(AppStateMachine.State.QR_DISPLAY, new Action(() => {
                setQRPicture();
                setControlVisible(qr_img, Visibility.Visible);
                setControlVisible(cancel_btn, Visibility.Collapsed);
                setControlVisible(radioLoad_img, Visibility.Collapsed); 
            }));
            stateMachine.setStateCallback(AppStateMachine.State.SCANNING, new Action(() => {
                setControlVisible(qr_img, Visibility.Collapsed);
                setControlVisible(cancel_btn, Visibility.Visible);
                setImg(padlock_Img, "ms-appx:///Assets/Resources/loadingRoll.gif", true);
            }));
            stateMachine.setStateCallback(AppStateMachine.State.UNLOCKED, new Action(() => {
                setControlVisible(qr_img, Visibility.Collapsed);
                setControlVisible(cancel_btn, Visibility.Collapsed);
                setControlVisible(reset_btn, Visibility.Visible);
                setImg(padlock_Img, "ms-appx:///Assets/Resources/unlocked-padlock.png");
            }));
            stateMachine.setStateCallback(AppStateMachine.State.CANCELLING, new Action(() => {
                setControlVisible(cancel_btn, Visibility.Collapsed);
                setControlVisible(cancelling_btn, Visibility.Visible);
                setControlEnabled(cancelling_btn, false);
            }));

            stateMachine.start();
        }

        private void cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            stateMachine.doEvent(AppStateMachine.Event.CANCEL_REQUEST); //cancelling...
            cancelTknSrc.Cancel();
        }

        private async void getQR_btn_Click(object sender, RoutedEventArgs e)
        {
            cancelTknSrc = new CancellationTokenSource();
            Task<HTTPResponse> qrTask = QRRequests.doGetQR(cancelTknSrc.Token);

            stateMachine.doEvent(AppStateMachine.Event.QR_REQUEST);

            HTTPResponse qrResponse = await qrTask;

            if (!qrResponse.wasCancelled)
            {
                imgData = qrResponse.bytes;

                stateMachine.doEvent(AppStateMachine.Event.GOT_QR);

                var progress = new Progress<int>();
                progress.ProgressChanged += Progress_ProgressChanged;

                cancelTknSrc = new CancellationTokenSource();
                try
                {
                    Task<int> getResultTask = QRRequests.doGetResult(cancelTknSrc.Token, progress);

                    int resultStatus = await getResultTask;

                    if (resultStatus == 2)
                        stateMachine.doEvent(AppStateMachine.Event.SCAN_ACCEPTED);
                    else if (resultStatus == 3)
                        stateMachine.doEvent(AppStateMachine.Event.SCAN_DENIED);


                }
                catch (OperationCanceledException) { stateMachine.doEvent(AppStateMachine.Event.CANCELLED); }
            }
            else stateMachine.doEvent(AppStateMachine.Event.CANCELLED);

        }

        private void Progress_ProgressChanged(object sender, int e)
        {
            if (e == 1)
                stateMachine.doEvent(AppStateMachine.Event.QR_SCANNED);
        }

        private void reset_btn_Click(object sender, RoutedEventArgs e)
        {
            stateMachine.doEvent(AppStateMachine.Event.RESET);
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

        internal async void setControlEnabled(ContentControl ctl, bool enabled)
        {
            await ctl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ctl.IsEnabled = enabled;
            });
        }


        internal async void setImg(Image image, string uri, bool animated = false)
        {
            await image.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => 
            {
                image.Source = null;
                if (animated)
                    AnimationBehavior.SetSourceUri(image, new Uri(uri));
                else
                    image.Source = new BitmapImage(new Uri(uri));
            });
        }


    }
}
