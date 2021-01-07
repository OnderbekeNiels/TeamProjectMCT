using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TempoTrack.Views.Onboarding
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnboardingKlassement : ContentPage
    {
        public OnboardingKlassement()
        {
            InitializeComponent();
            imgLogo.Source = ImageSource.FromResource("TempoTrack.Assets.Images.LogoOnboarding.png");
            btnContinue.Clicked += btnContinue_Clicked;
            btnReturn.Clicked += btnReturn_Clicked;
        }

        private void btnReturn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void btnContinue_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new OnboardingGPS());
        }
    }
}