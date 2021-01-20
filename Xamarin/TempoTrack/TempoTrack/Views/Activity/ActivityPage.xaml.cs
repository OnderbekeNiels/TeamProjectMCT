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
        RondesGebruiker RondeInfo;
        GebruikerV2 GebruikersInfo;
        public ActivityPage(RondesGebruiker parRondesGebruiker, GebruikerV2 parGebruikersInfo)
        {
            InitializeComponent();
            btnStoppen.Clicked += btnStoppen_clicked;

            RondeInfo = parRondesGebruiker;
            GebruikersInfo = parGebruikersInfo;

            /*if (GebruikersInfo.GebruikerId == RondeInfo.Admin)
            {
                //AdminControls tonen
                btnEtappeStoppen.IsVisible = true;
            }*/
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
                await EtappeRepository.UpdateDeelnemerGestopt(RondeInfo.RondeId, GebruikersInfo.GebruikerId);
                Navigation.PushAsync(new RondeOverzichtPage(GebruikersInfo));
            }
        }
    }
}