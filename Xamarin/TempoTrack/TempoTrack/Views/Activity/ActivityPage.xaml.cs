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
    public partial class ActivityPage : ContentPage
    {
        #region *** Global Variables ***

        EtappesRonde etappe;

        GebruikerV2 gebruikersInfo;

        List<LapTijd> timeRegistrations = new List<LapTijd>(); //Houd lokaal de laptijden bij.

        int laps = 0; //Houd bij hoeveel rondes er al zijn afgewerkt

        bool isRacing; //Houd bij of de etappe bezig is of niet.

        bool passedCheckpoint; //Houd bij of het checkpoint gepasseerd is of niet.

        int totalLaps; //Totale aantal rondes die gereden moeten worden

        bool isChecking; //Houd bij of de starttijd al gepasseerd is of niet.

        int timeDuring = 0; //Houd lap tijden bij om gemiddelde laptijd te tonen.

        double totaalBezig = 0; //Houd bij hoelang een activiteit al bezig is.

        double avgSeconds = 0; //Houd gemiddelde laptijd bij.

        #endregion

        public ActivityPage(EtappesRonde parEtappe, GebruikerV2 parGebruikersInfo)
        {
            InitializeComponent();
            btnStoppen.Clicked += btnStoppen_clicked;

            etappe = parEtappe;
            gebruikersInfo = parGebruikersInfo;
            totalLaps = etappe.Laps;

            isRacing = false;
            passedCheckpoint = false;
            isChecking = true;

            this.Title = etappe.EtappeNaam;

            checkAcknowledgementAsync();

        }

        #region *** Checking functions  ***

        private async Task checkAcknowledgementAsync()
        {
           bool answer =  await DisplayAlert("Belangrijke informatie", "Om deze app uw activiteit te laten opnemen is het vereist om uw gsm niet te vergrendelen en op deze pagina te blijven.", "OKE", "TERUG");
            if (answer)
            {
                DeviceDisplay.KeepScreenOn = true;
                Device.StartTimer(TimeSpan.FromSeconds(1), checkTimePassed);
            }
            else {
                Navigation.PopAsync();
            }
        }

        private bool checkTimePassed()
        {
            Debug.WriteLine("Checken voor start");
            TimeSpan timeBeforeStart = etappe.StartTijd.Subtract(DateTime.Now);
            lblRonde.Text = timeBeforeStart.ToString(@"hh\:mm\:ss");
            if (etappe.StartTijd <= DateTime.Now.ToLocalTime())
            {
                Debug.WriteLine("START");
                lblRonde.Text = $"0/{totalLaps}";
                lblAftelKlok.Text = "Rondes";
                isRacing = true;
                TriggerAsync();
                isChecking = false;

                // Timer
                Device.StartTimer(TimeSpan.FromSeconds(1), PageTimer);
            }

            return isChecking;
        }

        //Controleren of de finish of het checkpoint gepasseerd is.
        private async Task<bool> CheckIfPassed()
        {
            Location Phone = await GetCurrentLocation(); //Ophalen locatie van de gsm

            //Gegevens finish op de piste in Brugge
            Location finishP1 = new Location(51.205852955695214, 3.243597105208559); //Linker uiteinde finish piste
            Location finishP2 = new Location(51.20587312131655, 3.243555530972205); //Midden finsih piste
            Location finishP3 = new Location(51.205889085760525, 3.243516638944648); //Rechter uiteinde finish piste

            Location checkpoint = new Location(51.20551719426052, 3.244668291323074); //Midden checkpoint piste

            //Berekenen afstand van jouw gsm ten opzichte van de punten.
            double distanceFinishP1 = Location.CalculateDistance(finishP1, Phone, DistanceUnits.Kilometers) * 1000;
            double distanceFinishP2 = Location.CalculateDistance(finishP2, Phone, DistanceUnits.Kilometers) * 1000;
            double distanceFinishP3 = Location.CalculateDistance(finishP3, Phone, DistanceUnits.Kilometers) * 1000;

            double distanceCheckpoint = Location.CalculateDistance(checkpoint, Phone, DistanceUnits.Kilometers) * 1000;

            //Checken of je op 6 meter bent van het chechpoint
            if (distanceCheckpoint <= 7)
            {
                passedCheckpoint = true;
            }

            //Checken of je op 2 meter bent van de finsih 
            if (distanceFinishP1 <= 3 || distanceFinishP2 <= 3 || distanceFinishP3 <= 3)
            {
                //Kijken of je het checkpoint al gepasseerd bent
                if (passedCheckpoint)
                {
                    //Je bent binnen de range van de finish
                    passedCheckpoint = false; //Checkpoint weer op false voor komende ronde
                    return true;
                }
                else
                {
                    //Je bent niet aan het checkpoint gepasseerd -> ongeldige passage
                    return false;
                }
            }
            else
            {
                //Je bent niet aan de finish gepasseerd
                return false;
            }
        }

        //Controleren of de finish of het checkpoint overschreden zijn
        private async Task TriggerAsync()
        {
            while (isRacing)
            {
                if (!await CheckIfPassed())
                {
                    Debug.WriteLine("reading");
                }
                else
                {
                    laps++;
                    lblRonde.Text = $"{laps}/{totalLaps}";
                    Debug.WriteLine(timeDuring);
                    timeRegistrations.Add(new LapTijd(etappe.EtappeId, gebruikersInfo.GebruikerId, timeDuring, laps));
                    ShowAvgLaptijd();

                    if (laps == totalLaps)
                    {
                        isRacing = false;
                        SaveEtappeAsync();

                    }
                }
            }
        }

        #endregion


        #region *** Events ***

        //Toont de huidige tijd van de etappe in de UI.
        private bool PageTimer()
        {
            timeDuring++;
            Device.BeginInvokeOnMainThread(() =>
            {
                TimeSpan timeSinceStart = DateTime.Now - etappe.StartTijd;
                lblTotalTime.Text = $"{timeSinceStart.ToString(@"hh\:mm\:ss")}";
            });
            return isRacing;
        }

        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();

            //Alle checks afzetten.
            DeviceDisplay.KeepScreenOn = false;
            isRacing = false;
            isChecking = false;
            base.OnDisappearing();
        }

        //Toont de gemiddelde laptijd in de UI.
        private void ShowAvgLaptijd()
        {
            totaalBezig += timeDuring;

            avgSeconds = totaalBezig / laps;

            TimeSpan avgTime = TimeSpan.FromSeconds(avgSeconds);
            lblAvgLapTime.Text = avgTime.ToString(@"hh\:mm\:ss");

            timeDuring = 0;
        }

        private async Task SaveEtappeAsync()
        {
            checkConnectivity();
            //Scherm mag uit gaan want er zijn geen berekeningen en while loops meer
            DeviceDisplay.KeepScreenOn = false;
            await DisplayAlert("Etappe afgewerkt", "U heeft deze etappe afgewerkt, deze wordt nu opgeslagen en verwerkt. U wordt doorgestuurd naar de etappe pagina bij succes.", "SLUITEN");
            bool succes = await EtappeRepository.SaveRiddenEtappe(timeRegistrations);

            if (succes)
            {
                Navigation.PopAsync();
            }
            else
            {
                await ReSaveEtappeAsync();
            }
        }

        //Fallback bij eerste poging opslaan.
        private async Task ReSaveEtappeAsync()
        {
            bool answer = await DisplayAlert("Etappe NIET opgeslagen", "De etappe is niet opgeslagen, klik op probeer opnieuw.", "PROBEER OPNIEUW", "NIET OPSLAAN");
            if (answer)
            {
                bool succes = await EtappeRepository.SaveRiddenEtappe(timeRegistrations);
                if (succes)
                {
                    Navigation.PopAsync();
                }
                else
                {
                    await ReSaveEtappeAsync();
                }
            }
            else
            {
                Navigation.PopAsync();
            }
        }

        private void btnStoppen_clicked(object sender, EventArgs e)
        {
            Console.WriteLine("Ronde opgegeven");
            OpgevenRonde();
        }

        private async Task OpgevenRonde()
        {
            checkConnectivity();
            //pop up zodat we zeker zijn of de deelnemer wil stoppen met zijn etappe
            bool answer = await DisplayAlert("Opgepast", "Als u nu stopt met deelnemen kan u volgende etappes niet meer meerijden. Bent u zeker dat u wil stoppen met deelnemen?", "Ja", "Nee");
            Console.WriteLine("Answer: " + answer);
            if (answer == true)
            {
                isRacing = false;
                DeviceDisplay.KeepScreenOn = false;

                //Deelnemer uit de ronde smijten
                await EtappeRepository.UpdateDeelnemerGestopt(etappe.RondId, gebruikersInfo.GebruikerId);
                Navigation.PushAsync(new RondeOverzichtPage(gebruikersInfo));
            }
        }

        #endregion

        CancellationTokenSource cts;

        private async Task<Location> GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(1));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);
                if (location != null)
                {
                    Location Locationinate = new Location(location.Latitude, location.Longitude);
                    return Locationinate;
                }
                else
                {
                    return null;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                return null;
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                return null;
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                return null;
            }
            catch (Exception ex)
            {
                // Unable to get location
                return null;
            }
        }

        private void checkConnectivity()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                Navigation.PushModalAsync(new NoConnection());
            }
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }


    }
}