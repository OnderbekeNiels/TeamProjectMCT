using System;
using Xamarin.Essentials;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TempoTrack.Views.Onboarding
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnboardingGPS : ContentPage
    {
        public OnboardingGPS()
        {
            InitializeComponent();
            imgLogo.Source = ImageSource.FromResource("TempoTrack.Assets.Images.LogoOnboarding.png");
            btnToestaan.Clicked += btnToestaan_Clicked;
            btnReturn.Clicked += btnReturn_Clicked;
        }

        private void btnReturn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void btnToestaan_Clicked(object sender, EventArgs e)
        {
            //Toestemming vragen
            var permission = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
            if (permission == PermissionStatus.Granted)
            {
                //doorgaan
                Navigation.PushAsync(new OnboardingStart());
            }
            else if (permission != PermissionStatus.Granted) 
            {
                permission = await Permissions.RequestAsync<Permissions.LocationAlways>();
            }
        }
    }
}