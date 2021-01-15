using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TempoTrack.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TempoTrack.Views.Activity
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityPage : ContentPage
    {
        public Guid GebruikersId { get; set; }
        public Guid EtappeId { get; set; }

        //Globale variabelen

        List<LapTijd> timeRegistrations = new List<LapTijd>(); //Houd lokaal de laptijden bij.

        int laps = 0; //Houd bij hoeveel rondes er al zijn afgewerkt

        DateTime latestMeasurement; //Houd bij wanneer er voor het laatst gemeten is.

        bool isRacing; //Houd bij of de etappe bezig is of niet.

        bool passedCheckpoint; //Houd bij of het checkpoint gepasseerd is of niet.
        public ActivityPage()
        {
            GebruikersId = Guid.Parse("7D64AA90-966B-4BEF-87FE-7C0FD4BE2031");
            EtappeId = Guid.Parse("CBB74C13-66EB-4856-8AD6-BA74F67C0AAC");

            InitializeComponent();

            btnStoppen.Clicked += BtnStop_Clicked;

            StartTracking();
        }

        private void BtnStop_Clicked(object sender, EventArgs e)
        {
            isRacing = false;
            DeviceDisplay.KeepScreenOn = false; //De app mag weer automatisch sluimeren.
            string json = JsonConvert.SerializeObject(timeRegistrations);
        }

        private void StartTracking()
        {
            isRacing = true;
            passedCheckpoint = false;
            latestMeasurement = DateTime.Now;
            DeviceDisplay.KeepScreenOn = true; //Om de app werkend te houden moet het scherm opgelicht blijven.
            TriggerAsync(); //Starten met controleren of de meet gepasseerd is.
        }

        #region *** Manier 1: Pollen op geolocatie zonder geofencing lib. ***

        CancellationTokenSource cts;

        private async Task<Coord> GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(1));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);
                if (location != null)
                {
                    Coord coordinate = new Coord(location.Latitude, location.Longitude);
                    return coordinate;
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

        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
        }

        //Controleren of de finish of het checkpoint gepasseerd is.
        public async Task<bool> CheckIfPassed()
        {
            Coord currentL = await GetCurrentLocation(); //Ophalen locatie van de gsm
            Location Phone = new Location(currentL.Latitude, currentL.Longtitude);

            //Gegevens thuis
            Location finishP1 = new Location(50.915360140224536, 3.6511388192946113); //Linker uiteinde finish piste
            Location finishP2 = new Location(50.915356335370404, 3.6511052916846483); //Midden finsih piste
            Location finishP3 = new Location(50.915352530515946, 3.6510684113136893); //Rechter uiteinde finish piste

            Location checkpoint = new Location(50.91506396202935, 3.651253141698211); //Midden checkpoint piste

            //Berekenen afstand van jouw gsm ten opzichte van de punten.
            double distanceFinishP1 = Location.CalculateDistance(finishP1, Phone, DistanceUnits.Kilometers) * 1000;
            double distanceFinishP2 = Location.CalculateDistance(finishP2, Phone, DistanceUnits.Kilometers) * 1000;
            double distanceFinishP3 = Location.CalculateDistance(finishP3, Phone, DistanceUnits.Kilometers) * 1000;

            double distanceCheckpoint = Location.CalculateDistance(checkpoint, Phone, DistanceUnits.Kilometers) * 1000;

            //Checken of je op 6 meter bent van het chechpoint
            if (distanceCheckpoint <= 6)
            {
                passedCheckpoint = true;
            }

            //Checken of je op 2 meter bent van de finsih 
            if (distanceFinishP1 <= 2 || distanceFinishP2 <= 2 || distanceFinishP3 <= 2)
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
        public async Task TriggerAsync()
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
                    lblRonde.Text = laps.ToString();
                    int time = Convert.ToInt32((DateTime.Now - latestMeasurement).TotalSeconds);
                    timeRegistrations.Add(new LapTijd(EtappeId, GebruikersId, time, laps));
                    latestMeasurement = DateTime.Now;
                }
            }

        }

        #endregion
    }
}