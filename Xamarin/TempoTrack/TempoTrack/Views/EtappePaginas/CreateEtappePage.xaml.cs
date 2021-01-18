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
        private static RondesGebruiker RondeInfo { get; set; }
        private static GebruikerV2 GebruikersInfo { get; set; }
        public CreateEtappePage(RondesGebruiker rondeInfo, GebruikerV2 gebruikersInfo)
        {
            RondeInfo = rondeInfo;
            GebruikersInfo = gebruikersInfo;

            InitializeComponent();
            laps = 1;
            btnCreate.Clicked += btnCreate_clicked;

            //numberpicker
            btnUp.Clicked += btnUp_clicked;
            btnDown.Clicked += btnDown_clicked;

            tpEtappe.Time = DateTime.Now.AddMinutes(1).TimeOfDay;
            lblAantalRondes.Text = Convert.ToString(1);
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
                await DisplayAlert("Foutmelding", "De etappe kan niet in het verleden plaats vinden", "OK");
            }
            // controleren of er 1 of meerdere laps zijn
            if (laps < 1)
            {
                //alert
                await DisplayAlert("Foutmelding", "Er moet minstens 1 lap gereden worden", "OK");
            }
            //controleren als alles goed is
            if (date > DateTime.Now && laps > 0)
            {
                //dataverwerken in database
                Etappe etappe = new Etappe();
                etappe.Laps = laps;
                etappe.RondeId = RondeInfo.RondeId;
                etappe.LapAfstand = (float)(laps * 333.33);
                etappe.StartTijd = date;

                Etappe etappeResponse = await EtappeRepository.CreateEtappe(etappe);

                if (etappeResponse == null)
                {
                    //Foutmelding
                    await DisplayAlert("Foutmelding", "Er is iets foutgelopen bij het aanmaken van de etappe", "OK");
                }
                else
                {
                    //melding dat de ronde succesvol is aangemaakt
                    await DisplayAlert("Succes", "Etappe is succesvol aangemaakt", "OK");

                    //Etappe aangemaakt doorgaan naar etappe pagina
                    Navigation.PushAsync(new EtappeOverzichtPage(RondeInfo,GebruikersInfo));
                    //Navigation.PopAsync();
                }
            }
            //anders niets doen
        }
    }   
}