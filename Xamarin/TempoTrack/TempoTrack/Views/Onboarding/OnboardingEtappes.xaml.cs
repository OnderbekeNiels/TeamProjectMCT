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
    public partial class OnboardingEtappes : ContentPage
    {
        public OnboardingEtappes()
        {
            InitializeComponent();
            imgLogo.Source = ImageSource.FromResource("TempoTrack.Assets.Images.LogoOnboarding.png");
            btnContinue.Clicked += btnContinue_Clicked;
        }

        private void btnContinue_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new OnboardingKlassement());
        }
    }
}