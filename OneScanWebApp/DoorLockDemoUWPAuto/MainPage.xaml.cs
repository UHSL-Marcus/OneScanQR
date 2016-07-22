using HTTPRequestLibUWP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using XamlAnimatedGif;
using DoorLockDemoUWPAuto.ServiceReference1;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DoorLockDemoUWPAuto
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private House house = new House();
        private IhsSoapClient ihs = new IhsSoapClient();
        private int houseID = 1;
        private AppStateMachine stateMachine = new AppStateMachine();
        private byte[] imgData = new byte[0];
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            stateMachineInit();

            title_text.Text = "Door " + Consts.DoorID;

            InitializeHouseResponse x = await ihs.InitializeHouseAsync(houseID);

            house = x.Body.InitializeHouseResult;

            GetHouseResponse y = await ihs.GetHouseAsync(houseID);

            house = y.Body.GetHouseResult;

            while (true)
            {

                HTTPResponse QRresp = await QRRequests.doGetQR();

                imgData = QRresp.bytes;

                stateMachine.doEvent(AppStateMachine.Event.QR_RECIEVED);

                var progress = new Progress<int>();
                progress.ProgressChanged += Progress_ProgressChanged;

                int status = await QRRequests.doGetResult(progress);

                if (status == 2)
                {
                    house.Outside.Lights[0] = 1;
                    house.Outside.Lights[1] = 1;
                    house.Outside.FairyLights = true;
                    await ihs.SetHouseAsync(houseID, house);

                    stateMachine.doEvent(AppStateMachine.Event.SCAN_ACCEPTED);
                }
                else if (status == 3)
                    stateMachine.doEvent(AppStateMachine.Event.SCAN_DENIED);

                stateMachine.doEvent(AppStateMachine.Event.START_RESET);

                int count = 6;
                while (count > 0)
                {
                    reset_text.Text = count.ToString();
                    count--;
                    await Task.Delay(1000);
                }

                stateMachine.doEvent(AppStateMachine.Event.RESET);
                house.Outside.Lights[0] = 0;
                house.Outside.Lights[1] = 0;
                house.Outside.FairyLights = false;
                await ihs.SetHouseAsync(houseID, house);

            }

        }

        private void stateMachineInit()
        {
            stateMachine.setStateCallback(AppStateMachine.State.FETCHING_QR, new Action(() => {
                setControlVisible(qr_img, Visibility.Visible);
                setControlVisible(reset_VwBx, Visibility.Collapsed);
                setImg(qr_img, "ms-appx:///Assets/Resources/LoadingEllipsis.gif", true);
                setImg(padlock_Img, "ms-appx:///Assets/Resources/locked-padlock.png");
            }));

            stateMachine.setStateCallback(AppStateMachine.State.DISPLAYING_QR, new Action(() => {
                setQRImage();
            }));

            stateMachine.setStateCallback(AppStateMachine.State.SCANNING, new Action(() => {
                setImg(qr_img, "ms-appx:///Assets/Resources/LoadingEllipsis.gif", true);
            }));

            stateMachine.setStateCallback(AppStateMachine.State.SUCCESS, new Action(() => {
                setControlVisible(qr_img, Visibility.Collapsed);
                setImg(padlock_Img, "ms-appx:///Assets/Resources/unlocked-padlock.png");
            }));

            stateMachine.setStateCallback(AppStateMachine.State.FAILED, new Action(() => {
                setControlVisible(qr_img, Visibility.Collapsed);
                setImg(padlock_Img, "ms-appx:///Assets/Resources/locked-padlock.png");
            }));

            stateMachine.setStateCallback(AppStateMachine.State.RESETTING, new Action(() => {
                setControlVisible(reset_VwBx, Visibility.Visible);
            }));


            stateMachine.start();
        }

        private void Progress_ProgressChanged(object sender, int e)
        {
            if (e == 1)
                stateMachine.doEvent(AppStateMachine.Event.QR_SCANNED);
        }

        internal async void setQRImage()
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
