using Plugin.Geofence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace TeamProject_LocationTrigger.Models
{
    public class CrossGeofenceListener : IGeofenceListener
    {
        public void OnMonitoringStarted(string region)
        {
            Debug.WriteLine(string.Format("{0} - Monitoring started in region: {1}", CrossGeofence.Id, region));
        }

        public void OnMonitoringStopped()
        {
            Debug.WriteLine(string.Format("{0} - {1}", CrossGeofence.Id, "Monitoring stopped for all regions"));
        }

        public void OnMonitoringStopped(string identifier)
        {
            Debug.WriteLine(string.Format("{0} - {1}: {2}", CrossGeofence.Id, "Monitoring stopped in region", identifier));
        }

        public void OnError(string error)
        {
            Debug.WriteLine(string.Format("{0} - {1}: {2}", CrossGeofence.Id, "Error", error));
        }

        // Note that you must call CrossGeofence.GeofenceListener.OnAppStarted() from your app when you want this method to run.
        public void OnAppStarted()
        {
            Debug.WriteLine(string.Format("{0} - {1}", CrossGeofence.Id, "App started"));
        }

        public void OnRegionStateChanged(GeofenceResult result)
        {
            if(result.Transition == GeofenceTransition.Entered)
            {
                Debug.WriteLine($"Crossed finish at point -> {result.RegionId}");
                MessagingCenter.Send<CrossGeofenceListener>(this, "crossed");
            }

        }

        public void OnLocationChanged(GeofenceLocation location)
        {
            Debug.WriteLine($"Location changed {location.Latitude},  {location.Longitude}");
        }
    }
}
