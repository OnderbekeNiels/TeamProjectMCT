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
        GebruikerV2 gebruiker;
        public CreateRondePage(GebruikerV2 gebruikersInfo)
        {
            InitializeComponent();
            btnCreate.Clicked += btnCreate_cliked;
            gebruiker = gebruikersInfo;
        }

        private void btnCreate_cliked(object sender, EventArgs e)
        {
            Ronde ronde = new Ronde();
            ronde.Admin = gebruiker.GebruikerId;
            ronde.Naam = entRondeNaam.Text;

            RondeRepository.CreateRonde(ronde);
        }

    }
}