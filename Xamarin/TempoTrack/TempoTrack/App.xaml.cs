using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("DIN2014-Regular.ttf", Alias = "DIN2014-Regular")]
[assembly: ExportFont("DIN2014-Light.ttf", Alias = "DIN2014-Light")]
[assembly: ExportFont("DIN2014-Bold.ttf", Alias = "DIN2014-Bold")]

namespace TempoTrack
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (VersionTracking.IsFirstLaunchEver)
            {
                //Naar onboarding sturen
                MainPage = new NavigationPage(new Views.Onboarding.OnboardingEtappes())
                {
                    //Colors navbar
                    BarBackgroundColor = Color.FromHex("#016FB7"),
                    BarTextColor = Color.White,
                };
            }
            else
            {
                MainPage = new NavigationPage(new Views.Login.LoginPage())
                {
                    //Colors navbar
                    BarBackgroundColor = Color.FromHex("#016FB7"),
                    BarTextColor = Color.White,
                };

            }
        }

        protected override void OnStart()
        {
            VersionTracking.Track();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
