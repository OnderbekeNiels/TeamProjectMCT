using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoTrack.Models;
using TempoTrack.Repositories;
using TempoTrack.Views.InternetConnectivity;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TempoTrack.Views.RondePaginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeelnemersOverzichtPage : ContentPage
    {
        private static Guid RondeId;
        public DeelnemersOverzichtPage(Guid rondeId)
        {
            checkConnectivity();
            //Kleuren instellen voor navbar
            NavigationPage.SetHasBackButton(this, true);
            Color fireRed = Color.FromHex("#B22222");
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = fireRed;
            ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.White;

            RondeId = rondeId;

            InitializeComponent();

            btnRefresh.Clicked += BtnRefresh_Clicked;

            LoadDeelnemersAsync();
        }

        private async Task LoadDeelnemersAsync()
        {
            checkConnectivity();
            int teller = 0;
            List<DeelnemersRonde> deelnemers = await RondeRepository.GetDeelnemersRonde(RondeId);

            foreach(DeelnemersRonde item in deelnemers)
            {
                teller += 1;
                item.Nummer = teller;
            }

            lvwDeelnemers.ItemsSource = deelnemers;

            if(teller > 1)
            {
                lblTotaalDeelnemers.Text = $"{teller.ToString()} deelnemers";
            }
            else
            {
                lblTotaalDeelnemers.Text = $"{teller.ToString()} deelnemer";
            }
        }

        private void BtnRefresh_Clicked(object sender, EventArgs e)
        {
            lvwDeelnemers.ItemsSource = null;
            lblTotaalDeelnemers.Text = "";
            LoadDeelnemersAsync();
        }

        private void checkConnectivity()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                Navigation.PushModalAsync(new NoConnection());
            }
        }
    }
}