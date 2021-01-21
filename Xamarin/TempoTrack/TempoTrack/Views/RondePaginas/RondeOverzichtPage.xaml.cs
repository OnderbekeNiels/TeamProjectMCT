using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoTrack.Models;
using TempoTrack.Repositories;
using TempoTrack.Views.EtappePaginas;
using TempoTrack.Views.Login;
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
            btnRefresh.Clicked += BtnRefresh_Clicked;
            btnLogOut.Clicked += BtnLogOut_Clicked;

            LoadRondesAsync(GebruikersInfo.GebruikerId);
        }

        private void BtnLogOut_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LoginPage());
        }

        private void BtnRefresh_Clicked(object sender, EventArgs e)
        {
            lvwRondes.ItemsSource = null;
            lvwRondes.IsRefreshing = true;
            LoadRondesAsync(GebruikersInfo.GebruikerId);
        }

        //De rondes ophalen van een bepaalde gebruiker
        private async Task LoadRondesAsync(Guid gebruikerId)
        {
            List<RondesGebruiker> rondesGebruiker = await RondeRepository.GetRondesGebruiker(gebruikerId);

            if(rondesGebruiker == null || rondesGebruiker.Count == 0)
            {
                lvwRondes.EndRefresh();
                lvwRondes.IsVisible = false;

                lblNoData.IsVisible = true;
            }
            else
            {
                lvwRondes.IsVisible = true;
                lblNoData.IsVisible = false;
            }

            foreach (RondesGebruiker item in rondesGebruiker)
            {
                if(item.Admin == GebruikersInfo.GebruikerId)
                {
                    item.IsAdmin = true;
                }
                else
                {
                    item.IsAdmin = false;
                }
                //Debug.WriteLine(item.ToString());
            }

            lvwRondes.ItemsSource = rondesGebruiker;
            lvwRondes.EndRefresh();
        }

        private void LvwRondes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            RondesGebruiker ronde = lvwRondes.SelectedItem as RondesGebruiker;

            if (ronde != null)
            {
                Navigation.PushAsync(new EtappeOverzichtPage(ronde, GebruikersInfo));
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

                DeelnemerResponse deelnemerResponse = await RondeRepository.AddDeelnemer(deelnemer);

                if (deelnemerResponse == null)
                {
                    await DisplayAlert("Foutmedling", "Geen verbinding met de servers.", "OK");
                }
                else
                {
                    if (deelnemerResponse.RondeId != null)
                    {
                        await DisplayAlert("Succes", "Je doet nu mee aan de ronde", "OK");
                        //doorsturen naar ronde pagina
                        Navigation.PushAsync(new RondeOverzichtPage(GebruikersInfo));
                    }
                    else
                    {
                        await DisplayAlert("Foutmedling", deelnemerResponse.Message, "OK");
                    }


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