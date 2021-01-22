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
        private static RondesGebruiker RondeInfo { get; set; }
        private static GebruikerV2 GebruikersInfo { get; set; }

        bool admin = false;
        public DeelnemersOverzichtPage(RondesGebruiker rondeInfo, GebruikerV2 gebruikersInfo)
        {
            RondeInfo = rondeInfo;
            GebruikersInfo = gebruikersInfo;

            checkConnectivity();
            InitializeComponent();


            if (GebruikersInfo.GebruikerId == RondeInfo.Admin)
            {
                //Kleuren instellen voor navbar
                NavigationPage.SetHasBackButton(this, true);
                Color fireRed = Color.FromHex("#B22222");
                ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = fireRed;
                ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.White;
                btnRefresh.TextColor = fireRed;
                btnRefresh.BorderColor = fireRed;
                admin = true;
            }
            else
            {
                //Kleuren instellen voor navbar
                //NavigationPage.SetHasBackButton(this, true);
                Color Blauw = Color.FromHex("#015D99");
                ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Blauw;
                ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.White;
                btnRefresh.TextColor = Blauw;
                btnRefresh.BorderColor = Blauw;
                admin = false;
            }


                btnRefresh.Clicked += BtnRefresh_Clicked;

            LoadDeelnemersAsync();
        }

        private async Task LoadDeelnemersAsync()
        {
            checkConnectivity();
            int teller = 0;
            List<DeelnemersRonde> deelnemers = await RondeRepository.GetDeelnemersRonde(RondeInfo.RondeId);

            foreach(DeelnemersRonde item in deelnemers)
            {
                if(admin == true)
                {
                    item.Kleur = "#B22222";
                }
                else
                {
                    item.Kleur = "#015D99";
                }

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