using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TempoTrack.Views.InternetConnectivity
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoConnection : ContentPage
    {
        public NoConnection()
        {
            InitializeComponent();

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e) 
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    Navigation.PopModalAsync();
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

    }
}