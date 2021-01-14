using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TeamProject_LocationTrigger.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TeamProject_LocationTrigger
{
    public partial class MainPage : ContentPage
    {
        List<Coord> measuredList = new List<Coord>();

        public MainPage()
        {
            InitializeComponent();
            TriggerAsync();
        }


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

        public async Task<bool> CheckIfPassed()
        {
            Coord currentL = await GetCurrentLocation();
            lblCoordinate.Text = $"{currentL.Latitude}, {currentL.Longtitude}";

            //Gegevens piste brugge
            //Location Point1 = new Location(51.20585030789018, 3.2436127186772);
            //Location Point2 = new Location(51.205871733863816, 3.243559074497764);
            //Location Point3 = new Location(51.2058952604116, 3.2435081125272998);

            Location Point1 = new Location(50.91526320925043, 3.651294345010419);
            Location Point2 = new Location(50.91524376217434, 3.6511924210694904);
            Location Point3 = new Location(50.915230233768796, 3.651101896516691);
            Location Phone = new Location(currentL.Latitude, currentL.Longtitude);

            double distanceP1 = Location.CalculateDistance(Point1, Phone, DistanceUnits.Kilometers) * 1000;
            double distanceP2 = Location.CalculateDistance(Point2, Phone, DistanceUnits.Kilometers) * 1000;
            double distanceP3 = Location.CalculateDistance(Point3, Phone, DistanceUnits.Kilometers) * 1000;

            if (distanceP1 <= 3 || distanceP2 <= 3 || distanceP3 <= 3)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

       bool raceGoing = true;

        //public async Task TriggerAsync()
        //{
        //    TimeSpan lastMeasurement = DateTime.Now.TimeOfDay;
        //    int lap = 0;
        //    while (raceGoing)
        //    {
        //        if (!await CheckIfPassed())
        //        {
        //            lblPassed.Text = "Did not pass";
        //        }
        //        else if ((lastMeasurement + TimeSpan.FromSeconds(10)) <= DateTime.Now.TimeOfDay)
        //        {
        //            lastMeasurement = DateTime.Now.TimeOfDay;
        //            lap++;
        //            lblPassed.Text = $"Line passed";
        //            lblLap.Text = $"Crossed point: {lap} times";
        //            lblTimeOfLap.Text = $"{lastMeasurement}";

        //        }
        //        else
        //        {
        //            lblPassed.Text = "we zimme aant wachten";
        //        }
        //    }

        //}

        public async Task TriggerAsync()
        {
            int lap = 0;
            DateTime lastMeasurement = DateTime.Now;
            while (raceGoing)
            {
                if (!await CheckIfPassed())
                {
                    lblPassed.Text = "Did not pass line";
                }
                else
                {
                    if(lastMeasurement.AddSeconds(10) < DateTime.Now)
                    {
                        lap++;
                        lblPassed.Text = $"Line passed";
                        lblLap.Text = $"Crossed point: {lap} times";
                        lastMeasurement = DateTime.Now;
                    }
                }
            }

        }

       

    }
}
