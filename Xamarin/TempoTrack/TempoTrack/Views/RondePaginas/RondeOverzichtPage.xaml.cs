using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoTrack.Models;
using TempoTrack.Repositories;
using TempoTrack.Views.EtappePaginas;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TempoTrack.Views.RondePaginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RondeOverzichtPage : ContentPage
    {
        public static GebruikerV2 GebruikersInfo { get; set; }

        public RondeOverzichtPage(GebruikerV2 gebruikersInfo)
        {
            GebruikersInfo = gebruikersInfo;
            InitializeComponent();
            lvwRondes.ItemSelected += LvwRondes_ItemSelected;
            btnCreate.Clicked += btnCreate_Clicked;
            btnDeelnemen.Clicked += btnDeelnemen_Clicked;
            LoadRondesAsync(GebruikersInfo.GebruikerId, lvwRondes);
        }

        //De rondes ophalen van een bepaalde gebruiker
        private static async Task LoadRondesAsync(Guid gebruikerId, Xamarin.Forms.ListView lvw)
        {
            List<RondesGebruiker> rondesGebruiker = await RondeRepository.GetRondesGebruiker(gebruikerId);

            foreach (RondesGebruiker item in rondesGebruiker)
            {
                Debug.WriteLine(item.ToString());
                LoadKlassementAsync(item, lvw);
            }

            lvw.ItemsSource = rondesGebruiker;
        }

        //Het klassement ophalen van de rondes waarin de gebruiker meedeed.
        private static async Task LoadKlassementAsync(RondesGebruiker ronde, Xamarin.Forms.ListView lvw)
        {
            List<RondeKlassement> klassementen = await RondeRepository.GetRondeKlassement(ronde.RondeId);

            List<RondesGebruiker> output = new List<RondesGebruiker>();

            foreach (RondeKlassement item in klassementen)
            {
                Debug.WriteLine(item.ToString());
                if (item.GebruikersId == GebruikersInfo.GebruikerId)
                {
                    ronde.Ranking = item.Plaats;
                    ronde.GebruikersId = item.GebruikersId;
                    output.Add(ronde);
                }
            }

            foreach (RondesGebruiker item in output)
            {
                Debug.WriteLine(item.ToString());
            }

            //lvw.ItemsSource = output;
        }

        private void LvwRondes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            RondesGebruiker ronde = lvwRondes.SelectedItem as RondesGebruiker;

            if (ronde != null)
            {
                Navigation.PushAsync(new EtappeOverzichtPage(ronde));
                lvwRondes.SelectedItem = null;
            }
        }

        //Deelnemen aan een ronde
        private void btnDeelnemen_Clicked(object sender, EventArgs e)
        {
            askInviteCode();

        }

        //Invite code opvragen
        private async Task askInviteCode() 
        {
            string inviteCode = await DisplayPromptAsync("Deelnemen aan Ronde", "Voer de code van de ronde in waar je aan wilt deelnemen");

            if (inviteCode.Length == 8)
            {
                Guid gebruikersId = GebruikersInfo.GebruikerId;
                //Deelnemer toevoegen in database
                Deelnemer deelnemer = new Deelnemer();
                deelnemer.InviteCode = inviteCode;
                deelnemer.GebruikerId = gebruikersId;
                Debug.Write(deelnemer.GebruikerId);
                Deelnemer deelnemerResponse = await RondeRepository.AddDeelnemer(deelnemer);
                if (deelnemerResponse == null)
                {
                    await DisplayAlert("Foutmedling", "Foute invite code", "OK");
                }
                else
                {
                    //doorsturen naar ronde pagina
                    //Navigation.PushAsync(new EtappeOverzichtPage(deelnemerResponse.RondeId));
                }

            }
            else
            {
                //Code kan maar 8 tekens lang zijn
                await DisplayAlert("Foutmedling", "Foute invite code", "OK");
            }
        }

        //Navigeren naar create ronde pagina
        private void btnCreate_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateRondePage(GebruikersInfo));
        }
    }
}