using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using System.Text;

namespace NavigationSystem
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                return;
            }

            var stream = await FileSystem.OpenAppPackageFileAsync("map.html");
            var reader = new StreamReader(stream, Encoding.UTF8);
            string contents = reader.ReadToEnd();

            webView.Source = new HtmlWebViewSource() { Html = contents };




            //위치 이벤트, 종료될 때 해제해야함
            Geolocation.LocationChanged += LocationChanged;
            var request = new GeolocationListeningRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(1));
            await Geolocation.StartListeningForegroundAsync(request);

            //방위 이벤트
            if (Compass.IsSupported && !Compass.IsMonitoring)
            {
                Compass.ReadingChanged += CompassReadingChanged;
                Compass.Start(SensorSpeed.Fastest); // SensorSpeed 설정
            }
        }

        private void CompassReadingChanged(object? sender, CompassChangedEventArgs e)
        {
            heading.Text = MathF.Round((float)e.Reading.HeadingMagneticNorth).ToString() + "";
        }

        private async void LocationChanged(object? sender, GeolocationLocationChangedEventArgs e)
        {
            if (e.Location != null)
            {
                // 위치 정보 출력
                var latitude = e.Location.Latitude;
                var longitude = e.Location.Longitude;

                var horiziontalAccurancy = e.Location.Accuracy;
                var verticalAccuracy = e.Location.VerticalAccuracy;
                var altitude = e.Location.Altitude;
                var course = e.Location.Course;
                var speed = e.Location.Speed;
                var timestamp = e.Location.Timestamp;

                string data =
                    $"위도: {latitude}\n" +
                    $"경도: {longitude}\n" +
                    $"수평 정확도: {horiziontalAccurancy}m\n" +
                    $"수직 정확도: {verticalAccuracy}m\n" +
                    $"고도: {altitude}m\n" +
                    $"방위: {course}도\n" +
                    $"속도: {speed}m/s\n" +
                    $"수신시각: {timestamp}m/s";

                dataShow.Text = data;




                string script = $"updateMap({latitude}, {longitude}, {horiziontalAccurancy});";
                await webView.EvaluateJavaScriptAsync(script);
            }
        }
    }
}