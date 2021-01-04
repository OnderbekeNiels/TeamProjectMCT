using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoTrack.Views.Onboarding;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TempoTrack
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            //controleren of het de aller eerste launch is
            if (VersionTracking.IsFirstLaunchEver)
            {
                //Naar onboarding sturen
                Navigation.PushAsync(new OnboardingEteppes());
            }
            Navigation.PushAsync(new OnboardingEteppes());
        }
    }
}
