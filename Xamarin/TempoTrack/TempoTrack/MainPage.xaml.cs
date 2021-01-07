using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Newtonsoft.Json;
using Xamarin.Auth;
using System.Diagnostics;
using System.Net.Http;
using TempoTrack.Models;


namespace TempoTrack
{
    public partial class MainPage : ContentPage
    {
        private static string token = "";
        public MainPage()
        {
            InitializeComponent();

            //controleren of het de aller eerste launch is
            if (VersionTracking.IsFirstLaunchEver)
            {
                //Naar onboarding sturen
                Navigation.PushAsync(new Views.Onboarding.OnboardingEtappes());
            }
            else
            {
                Navigation.PushAsync(new Views.Login.LoginPage());
            }
        }

       
    }
}
