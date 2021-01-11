using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoTrack.Models;
using TempoTrack.Repositories;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TempoTrack.Views.EtappePaginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateEtappePage : ContentPage
    {
        int laps;
        Guid rondeId;
        public CreateEtappePage(Guid ronde)
        {
            InitializeComponent();
            laps = 0;
            btnCreate.Clicked += btnCreate_clicked;
            //numberpicker
            btnUp.Clicked += btnUp_clicked;
            btnDown.Clicked += btnDown_clicked;
            //rondeid 
            rondeId = ronde;
        }

        private void btnDown_clicked(object sender, EventArgs e)
        {
            laps--;
            lblAantalRondes.Text = laps.ToString();
        }

        private void btnUp_clicked(object sender, EventArgs e)
        {
            laps++;
            lblAantalRondes.Text = laps.ToString();
        }

        private void btnCreate_clicked(object sender, EventArgs e)
        {
            var tijd = tpEtappe.Time;
            var datum = dpEtappe.Date;
            var newDate = datum.Year.ToString() + "-" + datum.Month.ToString() + "-" + datum.Day.ToString() + " " + tijd.ToString();
            DateTime date = Convert.ToDateTime(newDate);
            Console.WriteLine(date);
            controleInput(date);
        }

        private async Task controleInput(DateTime date) 
        {
            // controleren of opgegeven data niet in het verleden ligt
            if (date < DateTime.Now)
            {
                //alert
                await DisplayAlert("Foutmedling", "De etappe kan niet in het verleden plaats vinden", "OK");
            }
            // controleren of er 1 of meerdere laps zijn
            if (laps < 1)
            {
                //alert
                await DisplayAlert("Foutmedling", "Rondes kunnen niet lager dan 1 zijn", "OK");
            }
            //controleren als alles goed is
            if (date > DateTime.Now && laps > 0)
            {
                //dataverwerken in database
                Etappe etappe = new Etappe();
                etappe.Laps = laps;
                etappe.RondeId = rondeId;
                etappe.LapAfstand = (float)(laps * 333.33);
                etappe.StartTijd = date;

                Etappe etappeResponse = await EtappeRepository.CreateEtappe(etappe);

                if (etappeResponse == null)
                {
                    //Foutmelding
                    await DisplayAlert("Foutmedling", "Er is iets foutgelopen bij het aanmaken van de ronde", "OK");
                }
                else
                {
                    //melding dat de ronde succesvol is aangemaakt
                    await DisplayAlert("Succes", "Ronde is succesvol aangemaakt", "OK");
                    //Etappe aangemaakt doorgaan naar etappe pagina
                    //Navigation.PushAsync();
                }

            }
            //anders niets doen
        }
    }
}