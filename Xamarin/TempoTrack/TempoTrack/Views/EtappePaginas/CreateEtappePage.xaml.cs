﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoTrack.Models;
using TempoTrack.Repositories;
using TempoTrack.Views.InternetConnectivity;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TempoTrack.Views.EtappePaginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateEtappePage : ContentPage
    {
        int laps;
        private static RondesGebruiker RondeInfo { get; set; }
        public CreateEtappePage(RondesGebruiker rondeInfo)
        {
            checkConnectivity();
            RondeInfo = rondeInfo;

            InitializeComponent();
            laps = 1;
            btnCreate.Clicked += btnCreate_clicked;

            //numberpicker
            btnUp.Clicked += btnUp_clicked;
            btnDown.Clicked += btnDown_clicked;

            tpEtappe.Time = DateTime.Now.AddMinutes(1).TimeOfDay;
            lblAantalRondes.Text = Convert.ToString(1);

            this.Title = rondeInfo.RondeNaam;

            //Kleuren instellen voor navbar
            NavigationPage.SetHasBackButton(this, true);
            Color fireRed = Color.FromHex("#B22222");
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = fireRed;
            ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.White;

            
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
            checkConnectivity();
            var tijd = tpEtappe.Time;
            var datum = dpEtappe.Date;
            var newDate = datum.Year.ToString() + "-" + datum.Month.ToString() + "-" + datum.Day.ToString() + " " + tijd.ToString();
            DateTime date = Convert.ToDateTime(newDate);
            Console.WriteLine(date);
            controleInput(date);
        }

        private async Task controleInput(DateTime date) 
        {
            checkConnectivity();
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
                    Navigation.PopAsync();

                }
            }
            //anders niets doen
        }

        private void checkConnectivity()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                Navigation.PushModalAsync(new NoConnection());
            }
        }
    }   
}