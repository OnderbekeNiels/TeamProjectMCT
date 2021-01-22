using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoTrack.Models;
using TempoTrack.Repositories;
using TempoTrack.Views.Activity;
using TempoTrack.Views.InternetConnectivity;
using TempoTrack.Views.RondePaginas;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TempoTrack.Views.EtappePaginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EtappeOverzichtPage : ContentPage
    {
        private static RondesGebruiker RondeInfo { get; set; }
        private static GebruikerV2 GebruikersInfo { get; set; }

        private static int etappeTeller = 0;

        private static int NavigationStackCount = 0;
        public EtappeOverzichtPage(RondesGebruiker rondeInfo, GebruikerV2 gebruikersInfo)
        {
            checkConnectivity();
            RondeInfo = rondeInfo;
            GebruikersInfo = gebruikersInfo;

            InitializeComponent();

            //UserControls
            btnInvite.Clicked += BtnInvite_Clicked;
            btnStoppen.Clicked += BtnStoppen_Clicked;
            btnRefresh.Clicked += BtnRefresh_Clicked;

            //AdminControls
            btnCreateEtappe.Clicked += BtnCreateEtappe_Clicked;
            btnInviteAdmin.Clicked += BtnInviteAdmin_Clicked;
            btnVerwijder.Clicked += BtnVerwijder_Clicked;
            lvwEtappesAdmin.ItemSelected += LvwEtappesAdmin_ItemSelected;

            btnDeelnemers.Clicked += BtnDeelnemers_Clicked;

            if(GebruikersInfo.GebruikerId == RondeInfo.Admin)
            {
                //Kleuren instellen voor navbar
                NavigationPage.SetHasBackButton(this, true);
                Color fireRed = Color.FromHex("#B22222");
                ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = fireRed;
                ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.White;

                //AdminControls tonen
                btnCreateEtappe.IsVisible = true;
                btnInviteAdmin.IsVisible = true;
                btnVerwijder.IsVisible = true;
                lvwEtappesAdmin.IsVisible = true;

                //AdminColors
                btnDeelnemers.TextColor = fireRed;
                btnDeelnemers.BorderColor = fireRed;
                btnRefresh.TextColor = fireRed;
                btnRefresh.BorderColor = fireRed;
                btnCreateEtappe.TextColor = fireRed;
                btnCreateEtappe.BorderColor = fireRed;
                btnInviteAdmin.TextColor = fireRed;
                btnInviteAdmin.BorderColor = fireRed;
                btnVerwijder.BackgroundColor = fireRed;
                lblNoData.TextColor = fireRed;

                //UserControls niet tonen
                btnStoppen.IsVisible = false;
                btnInvite.IsVisible = false;
                grdUserStanding.IsVisible = false;
                lvwEtappes.IsVisible = false;
            }
            else
            {
                //Kleuren instellen voor navbar
                //NavigationPage.SetHasBackButton(this, true);
                Color Blauw = Color.FromHex("#015D99");
                ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Blauw;
                ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.White;
            }

            LoadEtappesAsync();

            LoadTitle();
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new RondeOverzichtPage(GebruikersInfo));
            return true;
        }

        protected override async void OnDisappearing()
        {
            if (NavigationStackCount >= Navigation.NavigationStack.Count) 
            {
                Navigation.PushAsync(new RondeOverzichtPage(GebruikersInfo));
            }
        }

        protected override void OnAppearing()
        {
            //Tonen van etappe bij terecht komen op deze pagina 
            NavigationStackCount = Navigation.NavigationStack.Count;
            LoadEtappesAsync();
            base.OnAppearing();
        }

        private void LvwEtappesAdmin_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            EtappesRonde etappe = lvwEtappesAdmin.SelectedItem as EtappesRonde;
            if(etappe != null)
            {
                if (etappe.EtappeActief)
                {
                    Navigation.PushAsync(new ActivityPageAdmin(etappe));
                }
                else
                {
                    lvwEtappesAdmin.SelectedItem = null;
                }
            }
            else
            {
                lvwEtappesAdmin.SelectedItem = null;
            }
        }

        private void LoadTitle()
        {
            checkConnectivity();
            if(RondeInfo.RondeNaam.Length >= 20)
            {
                lblRondeNaam.FontSize = 16;
            }
            else
            {
                lblRondeNaam.FontSize = 24;
            }
            lblRondeNaam.Text = RondeInfo.RondeNaam;
        }

        private async Task LoadEtappesAsync()
        {
            checkConnectivity();
            List<EtappesRonde> etappes = await EtappeRepository.GetEtappesRonde(RondeInfo.RondeId, GebruikersInfo.GebruikerId);

            if(etappes == null || etappes.Count == 0)
            {
                lvwEtappes.EndRefresh();
                lvwEtappes.IsVisible = false;

                lblNoData.IsVisible = true;
            }
            else
            {
                lblNoData.IsVisible = false;
                lvwEtappes.IsVisible = true;
            }

            etappeTeller += etappes.Count();

            foreach (EtappesRonde item in etappes)
            {
                item.EtappeNaam = $"Etappe {etappeTeller}";
                etappeTeller -= 1;
               
            }

            if (GebruikersInfo.GebruikerId != RondeInfo.Admin)
            {
                lvwEtappes.ItemsSource = etappes;
                lblRondePlaats.Text = RondeInfo.Ranking;
                lblRondeTijd.Text = TimeSpan.FromSeconds(RondeInfo.TotaalTijd).ToString();
            }
            else
            {
                lvwEtappesAdmin.ItemsSource = etappes;
            }

            lvwEtappes.EndRefresh();
            lvwEtappesAdmin.EndRefresh();
        }

        private void BtnStoppen_Clicked(object sender, EventArgs e)
        {
            RemoveSpelerRonde(GebruikersInfo.GebruikerId, RondeInfo.RondeId);
        }

        private async Task RemoveSpelerRonde(Guid gebruikersId,Guid rondeId)
        {
            checkConnectivity();
            //Een confirmatie vragen
            bool confirmatie = await DisplayAlert("Waarschuwing", "Ben je zeker dat je de ronde wilt verlaten?", "Ja", "Nee");

            if(confirmatie == true)
            {
                int response = await EtappeRepository.RemoveDeelnemerRonde(GebruikersInfo.GebruikerId, RondeInfo.RondeId);

                if (response == 1)
                {
                    await DisplayAlert("Ronde verlaten", "Je ben succesvol uit de ronde verwijderd", "Ok");
                    await Navigation.PushAsync(new RondeOverzichtPage(GebruikersInfo));
                }
                else
                {
                    await DisplayAlert("Fout", "Er is een fout opgetreden bij het verlaten", "Ok");
                }
            }
            else
            {

            }

        }

        private void BtnInvite_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new Invite)
            PopUpInviteCode();
        }

        private async Task PopUpInviteCode() 
        {
            await DisplayAlert("Invite code", RondeInfo.InviteCode, "Ok");
        }

        private void BtnRefresh_Clicked(object sender, EventArgs e)
        {
            checkConnectivity();
            lvwEtappes.ItemsSource = null;
            lblRondeTijd.Text = "";
            lblRondePlaats.Text = "";
            LoadEtappesAsync();
            LoadRondesAsync(RondeInfo.RondeId, lblRondePlaats, lblRondeTijd);
        }

        private static async Task LoadRondesAsync(Guid rondeId, Xamarin.Forms.Label lblPlaats, Xamarin.Forms.Label lblTijd)
        {
            List<RondeKlassement> rondesKlassement = await RondeRepository.GetRondeKlassement(rondeId);
            foreach(RondeKlassement item in rondesKlassement)
            {
                if(item.GebruikersId == GebruikersInfo.GebruikerId)
                {
                    RondeInfo.Plaats = item.Plaats;
                    RondeInfo.TotaalTijd = item.TotaalTijd;

                    lblTijd.Text = TimeSpan.FromSeconds(RondeInfo.TotaalTijd).ToString();
                    lblPlaats.Text = RondeInfo.Ranking;

                }
            }
        }

        private void BtnVerwijder_Clicked(object sender, EventArgs e)
        {
            Ronde delRonde = new Ronde();
            delRonde.RondeId = RondeInfo.RondeId;

            DeleteRondes(delRonde.RondeId);
        }

        private async Task DeleteRondes(Guid rondeId)
        {
            checkConnectivity();
            bool confirmatie = await DisplayAlert("Waarschuwing", "Ben je zeker dat je de ronde wilt verwijderen?", "Ja", "Nee");

            if(confirmatie == true)
            {
                int response = await RondeRepository.DeleteRonde(rondeId);

                if (response == 1)
                {
                    await DisplayAlert("Ronde verwijderd", "De ronde is succesvol verwijderd", "Ok");
                    await Navigation.PushAsync(new RondeOverzichtPage(GebruikersInfo));
                }
                else
                {
                    await DisplayAlert("Fout", "Er is een fout opgetreden bij het verwijderen", "Ok");
                }
            }
            else
            {

            }
        }

        private void BtnInviteAdmin_Clicked(object sender, EventArgs e)
        {
            PopUpInviteCode();
        }

        private void BtnCreateEtappe_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateEtappePage(RondeInfo, GebruikersInfo));
        }

        private void lvwEtappes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            EtappesRonde etappe = lvwEtappes.SelectedItem as EtappesRonde;
            if(etappe != null)
            {
                if (etappe.StartTijd > DateTime.Now)
                {
                    if (etappe.EtappeActief)
                    {
                        Navigation.PushAsync(new ActivityPage(etappe, GebruikersInfo));
                    }
                    else
                    {
                        lvwEtappes.SelectedItem = null;
                    }
                }
                else
                {
                    lvwEtappes.SelectedItem = null;
                }
            }
            else
            {
                lvwEtappes.SelectedItem = null;
            }
            
        }

        private void BtnDeelnemers_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DeelnemersOverzichtPage(RondeInfo, GebruikersInfo));
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