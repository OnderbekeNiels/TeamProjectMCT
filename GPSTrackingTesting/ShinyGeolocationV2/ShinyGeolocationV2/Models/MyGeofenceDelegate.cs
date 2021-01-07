using Shiny.Locations;
using Shiny.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using ShinyGeolocationV2;
using Xamarin.Forms;

namespace ShinyGeolocationV2.Models
{
    public class MyGeofenceDelegate : IGeofenceDelegate
    {
        readonly INotificationManager notifications;

        public MyGeofenceDelegate(INotificationManager notifications)
        {
            this.notifications = notifications;
        }


        public async Task OnStatusChanged(GeofenceState newStatus, GeofenceRegion region)
        {
            await Application.Current.MainPage.DisplayAlert(newStatus.ToString(), $"{region.Identifier}", "Ok");
            Debug.WriteLine("Je bent binnen");

            if (newStatus == GeofenceState.Entered)
            {
                Debug.WriteLine("Je bent binnen");
                await this.notifications.Send(new Notification
                {
                    Title = "WELCOME!",
                    Message = "It is good to have you back " + region.Identifier
                });
            }
            else
            {
                await this.notifications.Send(new Notification
                {
                    Title = "GOODBYE!",
                    Message = "You will be missed at " + region.Identifier
                });
            }
        }



        //Task IGeofenceDelegate.OnStatusChanged(GeofenceState newStatus, GeofenceRegion region)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
