using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoTrack.Models;
using TempoTrack.Repositories;
using TempoTrack.Views.Activity;
using TempoTrack.Views.RondePaginas;
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
        public EtappeOverzichtPage(RondesGebruiker rondeInfo, GebruikerV2 gebruikersInfo)
        {
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

            if(GebruikersInfo.GebruikerId == RondeInfo.Admin)
            {
                //AdminControls tonen
                btnCreateEtappe.IsVisible = true;
                btnInviteAdmin.IsVisible = true;
                btnVerwijder.IsVisible = true;

                //UserControls niet tonen
                btnStoppen.IsVisible = false;
                btnInvite.IsVisible = false;
            }

            LoadEtappesAsync(RondeInfo.RondeId, RondeInfo.GebruikersId, lvwEtappes, lblRondePlaats, lblRondeTijd);
            Title = RondeInfo.RondeNaam;
        }

        private static async Task LoadEtappesAsync(Guid rondeId, Guid gebruikersId, Xamarin.Forms.ListView lvw, Xamarin.Forms.Label lblRondePlaats, Xamarin.Forms.Label lblRondeTijd)
        {
           List<EtappesRonde> etappes = await EtappeRepository.GetEtappesRonde(rondeId, gebruikersId);

            etappeTeller += etappes.Count();

            foreach (EtappesRonde item in etappes)
            {
                item.EtappeNaam = $"Etappe {etappeTeller}";
                etappeTeller -= 1;
                
                //item.TotaalRondeTijd = RondeInfo.TotaalTijd;
                //Debug.WriteLine("-------------------------------------------------------");
                //Debug.WriteLine(item.ToString());
                //Debug.WriteLine("-------------------------------------------------------");
            }

            //etappes.Reverse();

            lvw.ItemsSource = etappes;
            lblRondePlaats.Text = RondeInfo.Ranking;
            lblRondeTijd.Text = TimeSpan.FromSeconds(RondeInfo.TotaalTijd).ToString();
        }

        private void BtnStoppen_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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
            lvwEtappes.ItemsSource = null;
            lblRondeTijd.Text = "";
            lblRondePlaats.Text = "";
            LoadEtappesAsync(RondeInfo.RondeId, RondeInfo.GebruikersId, lvwEtappes, lblRondePlaats, lblRondeTijd);
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
            int response = await RondeRepository.DeleteRonde(rondeId);
            
            if(response == 1)
            {
                await DisplayAlert("Ronde verwijderd", "De ronde is succesvol verwijderd", "Ok");
                await Navigation.PushAsync(new RondeOverzichtPage(GebruikersInfo));
            }
            else
            {
                await DisplayAlert("Fout", "Er is een fout opgetreden bij het verwijderen", "Ok");
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
            if (etappe.StartTijd > DateTime.Now)
            {
               Navigation.PushAsync(new ActivityPage(RondeInfo.RondeId, GebruikersInfo));
            }
        }
    }
}