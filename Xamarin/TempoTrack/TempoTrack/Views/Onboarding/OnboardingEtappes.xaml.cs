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
    public partial class OnboardingEteppes : ContentPage
    {
        public OnboardingEteppes()
        {
            InitializeComponent();
            //imgLogo.Source = ImageSource.FromResource("TempoTrack.Assets.LogoOnboarding.svg");
        }
    }
}