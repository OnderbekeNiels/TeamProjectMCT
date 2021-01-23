using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TempoTrack.Models;
using TempoTrack.Repositories;
using TempoTrack.Views.InternetConnectivity;
using TempoTrack.Views.RondePaginas;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TempoTrack.Views.Activity
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityPageAdmin : ContentPage
    {
        #region *** Global Variables ***

        EtappesRonde etappe;

        bool isNotStarted;

        bool isNotStopped;

        #endregion

        public ActivityPageAdmin(EtappesRonde parEtappe)
        {
            checkConnectivity();
            InitializeComponent();
            btnStoppen.Clicked += BtnStoppen_Clicked; ;

            
            etappe = parEtappe;
            lblTotalTimeFixed.Text = "Tijd voor de start";
            this.Title = etappe.EtappeNaam;
            btnStoppen.IsEnabled = false;
        }

        private void BtnStoppen_Clicked(object sender, EventArgs e)
        {
            StopEtappe();
        }

        private bool TimeFromStartTimer()
        {
            if (DateTime.Now >= etappe.StartTijd)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    lblTotalTimeFixed.Text = "Duratie Etappe";
                    btnStoppen.IsEnabled = true;
                });

                isNotStopped = true;
                Device.StartTimer(TimeSpan.FromSeconds(1), TimeSinceStartTimer);
                isNotStarted = false;
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    TimeSpan timeToGo = etappe.StartTijd - DateTime.Now;
                    lblTotalTime.Text = $"{timeToGo.ToString(@"hh\:mm\:ss")}";
                });
            }
            return isNotStarted;
        }

        private bool TimeSinceStartTimer()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                TimeSpan timeSinceStart = DateTime.Now - etappe.StartTijd;
                lblTotalTime.Text = $"{timeSinceStart.ToString(@"hh\:mm\:ss")}";
            });

            return isNotStopped;
        }


        private async Task StopEtappe()
        {
            checkConnectivity();
            bool answer = await DisplayAlert("Etappe stoppen", "Bent u zeker dat u deze etappe wil stoppen? Doe dit enkel wanneer elke deelnemer klaar is met zijn etappe.", "JA", "NEE");
            if (answer)
            {
                // API CALL ETAPPE NIET ACTIEF ZETTEN
                bool succes = await EtappeRepository.StopEtappe(etappe);
                if (succes)
                {
                    DisplayAlert("Etappe gestopt", "U heeft deze etappe beeïndigd", "OKE");
                    isNotStopped = false;
                    Navigation.PopAsync();
                }
                else
                {
                    DisplayAlert("Etappe niet gestopt", "Er liep iets mis, porbeer opnieuw door nogmaals op stop etappe te klikken", "OKE");
                }
            }
           
        }

        protected override void OnDisappearing()
        {
            //Alle checks afzetten.
            isNotStarted = false;
            isNotStopped = false;
            base.OnDisappearing();
        }

        protected override void OnAppearing()
        {
            isNotStarted = true;
            Device.StartTimer(TimeSpan.FromSeconds(1), TimeFromStartTimer);
            base.OnAppearing();
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