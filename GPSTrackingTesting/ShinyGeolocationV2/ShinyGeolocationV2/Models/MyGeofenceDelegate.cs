using Shiny.Locations;
using Shiny.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using ShinyGeolocationV2;
using Xamarin.Forms;
using Shiny;

namespace ShinyGeolocationV2.Models
{
    public class MyGeofenceDelegate : IGeofenceDelegate, IShinyStartupTask
    {

        readonly CoreDelegateServices services;
        public MyGeofenceDelegate(CoreDelegateServices services) => this.services = services;


        public async Task OnStatusChanged(GeofenceState newStatus, GeofenceRegion region)
        {
            Debug.WriteLine("Ca marche wi matje");
            await this.services.Notifications.Send(
                this.GetType(),
                newStatus == GeofenceState.Entered,
                "Geofence Event",
                $"{region.Identifier} was {newStatus}"
            );
        }


        public void Start()
            => this.services.Notifications.Register(this.GetType(), true, "Geofences");
    }
}
