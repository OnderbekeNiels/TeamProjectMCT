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
    public partial class OnboardingStart : ContentPage
    {
        public OnboardingStart()
        {
            InitializeComponent();
            imgLogo.Source = ImageSource.FromResource("TempoTrack.Assets.Images.LogoOnboarding.png");
            btnStart.Clicked += btnStart_Clicked;
        }

        private void btnStart_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync();
            Console.WriteLine("Started");
        }
    }
}