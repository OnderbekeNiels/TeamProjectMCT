using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoTrack.Models;
using TempoTrack.Repositories;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TempoTrack.Views.RondePaginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateRondePage : ContentPage
    {
        GebruikerV2 GebruikersInfo;
        public CreateRondePage(GebruikerV2 gebruikersInfo)
        {
            InitializeComponent();
            btnCreate.Clicked += btnCreate_cliked;
            GebruikersInfo = gebruikersInfo;
        }

        private void btnCreate_cliked(object sender, EventArgs e)
        {
            
            Ronde ronde = new Ronde();
            ronde.Admin = GebruikersInfo.GebruikerId;
            //Controleren of er degelijk een ronde naam ingevuld is
            if (entRondeNaam.Text != null)
            {
                ronde.Naam = entRondeNaam.Text;
                CreateRonde(ronde);
            }
            else 
            {
                //Foutmelding geen entry
                PopUpError();
            }
        }

        private async Task PopUpError()
        {
            await DisplayAlert("Foutmedling", "Gelieve een ronde naam in te vullen", "OK");
        }

        private async Task CreateRonde(Ronde ronde)
        {
            Ronde rondeResponse = await RondeRepository.CreateRonde(ronde);
            //Controleren of de ronde correct is aangemaakt
            if (rondeResponse == null)
            {
                //Foutmelding
                await DisplayAlert("Foutmedling", "Er is iets foutgelopen bij het aanmaken van de ronde", "OK");
            }
            else 
            {
                //melding dat de ronde succesvol is aangemaakt
                await DisplayAlert("Succes", "Ronde is succesvol aangemaakt", "OK");

                //Ronde aangemaakt doorgaan naar etappe pagina
                Navigation.PushAsync(new RondeOverzichtPage(GebruikersInfo));
            }

        }

    }
}