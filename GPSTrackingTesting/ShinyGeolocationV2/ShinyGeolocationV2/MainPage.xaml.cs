using Shiny;
using Shiny.Locations;
using Shiny.Notifications;
<<<<<<< HEAD
using ShinyGeolocationV2.Models;
=======
>>>>>>> GPSTrackingWithShiny
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ShinyGeolocationV2
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            btnStart.Clicked += BtnStart_Clicked;
            btnStop.Clicked += BtnStop_Clicked;

            // shiny doesn't usually manage your viewmodels, so we'll do this for now
            var geofences = ShinyHost.Resolve<IGeofenceManager>();
            var notifications = ShinyHost.Resolve<INotificationManager>();

<<<<<<< HEAD
            //MyGeofenceDelegate objectToSubscribeTo = new MyGeofenceDelegate();
            //objectToSubscribeTo.Entered += EnteredGeofence;


=======

           
>>>>>>> GPSTrackingWithShiny
            Register = new Command(async () =>
            {

               
                // this is really only required on iOS, but do it to be safe
                var access = await notifications.RequestAccess();
                try
                {
                    if (access == AccessState.Available)
                    {
                        await geofences.StartMonitoring(new GeofenceRegion(
                            "Home",
<<<<<<< HEAD
                            new Position(50.916186692014556, 3.6510653463806473),
                            Distance.FromMeters(30))
=======
                            new Position(50.91510937218809, 3.6512293828729945),
                            Distance.FromMeters(10))
>>>>>>> GPSTrackingWithShiny
                        {
                            NotifyOnEntry = true,
                            NotifyOnExit = true,
                            SingleUse = false
                        });
                        Debug.WriteLine("Started Monitoring");
                        lblMessage.Text = "Started Monitoring";
<<<<<<< HEAD
                        TriggerAsync();
=======
>>>>>>> GPSTrackingWithShiny
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"ERROR: {ex.Message}");
                }

            });

            Stop = new Command(async () =>
            {
                try
                {
                        await geofences.StopAllMonitoring();
                        lblMessage.Text = "Stopped Monitoring";
<<<<<<< HEAD
                         raceGoing = false;
=======
>>>>>>> GPSTrackingWithShiny
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"ERROR: {ex.Message}");
                }

            });

        }

<<<<<<< HEAD
        //public void EnteredGeofence(object sender, EventArgs e)
        //{
        //    lblMessage.Text = "ENTERED";
        //}
=======
        public void NotifyMe()
        {
            lblMessage.Text = "ENTERED";
        }
>>>>>>> GPSTrackingWithShiny

        private void BtnStop_Clicked(object sender, EventArgs e)
        {
            Stop.Execute(null);
        }

        private void BtnStart_Clicked(object sender, EventArgs e)
        {
            //Toestemming vragen
            AskPermitionsAsync();
        }

        public async Task AskPermitionsAsync()
        {
            var permission = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
            if (permission == PermissionStatus.Granted)
            {
                Register.Execute(null);
            }
            else if (permission != PermissionStatus.Granted)
            {
                permission = await Permissions.RequestAsync<Permissions.LocationAlways>();
            }
        }

<<<<<<< HEAD


        public ICommand Register { get; }
        public ICommand Stop { get; }



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

        bool raceGoing = true;
        public async Task TriggerAsync()
        {
            int lap = 0;
            DateTime lastMeasurement = DateTime.Now;
            Debug.WriteLine($"---------------------------TESTING---------------------------");
            while (raceGoing)
            {
                Coord c = await GetCurrentLocation();
                Debug.WriteLine($"{c.Latitude}, {c.Longtitude}");
            }

        }





=======
        public ICommand Register { get; }
        public ICommand Stop { get; }
>>>>>>> GPSTrackingWithShiny
    }

}

