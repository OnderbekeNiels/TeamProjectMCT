using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoTrack.Models;
using TempoTrack.Repositories;
using TempoTrack.Views.RondePaginas;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TempoTrack.Views.Activity
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityPage : ContentPage
    {
        //Navigation.PushAsync(new ActivityPage(RondeInfo.rondeId, GebruikersInfo.gerbruikersId));
        Guid rondeId;
        GebruikerV2 gebruikersInfo;
        public ActivityPage(Guid parRondeId, GebruikerV2 parGebruikersInfo)
        {
            InitializeComponent();
            btnStoppen.Clicked += btnStoppen_clicked;

            rondeId = parRondeId;
            gebruikersInfo = parGebruikersInfo;
        }

        private void btnStoppen_clicked(object sender, EventArgs e)
        {
            Console.WriteLine("Ronde opgegeven");
            opgevenRonde();
        }

        private async Task opgevenRonde()
        {
            //pop up zodat we zeker zijn of de deelnemer wil stoppen met zijn etappe
            bool answer = await DisplayAlert("Opgepast", "Als u nu stopt met deelnemen kan u volgende etappes niet meer meerijden. Bent u zeker dat u wil stoppen met deelnemen?", "Ja", "Nee");
            Console.WriteLine("Answer: " + answer);
            if (answer == true)
            {
                //Deelnemer uit de ronde smijten
                await EtappeRepository.UpdateDeelnemerGestopt(rondeId, gebruikersInfo.GebruikerId);
                Navigation.PushAsync(new RondeOverzichtPage(gebruikersInfo));
            }
        }
    }
}