using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoTrack.Models;
using TempoTrack.Repositories;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TempoTrack.Views.Activity
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityPage : ContentPage
    {
        //Navigation.PushAsync(new ActivityPage(RondeInfo.rondeId, GebruikersInfo.gerbruikersId));
        Guid rondeId;
        Guid gebruikersId;
        public ActivityPage(Guid parRondeId, Guid parGebruikersId)
        {
            InitializeComponent();
            btnStoppen.Clicked += btnStoppen_clicked;

            rondeId = parRondeId;
            gebruikersId = parGebruikersId;
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
                await EtappeRepository.UpdateDeelnemerGestopt(rondeId, gebruikersId);
                Navigation.PopAsync();
            }
        }

    }
}