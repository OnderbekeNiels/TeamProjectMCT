using Shiny;
using Shiny.Locations;
using Shiny.Notifications;
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
                            new Position(50.91510937218809, 3.6512293828729945),
                            Distance.FromMeters(10))
                        {
                            NotifyOnEntry = true,
                            NotifyOnExit = true,
                            SingleUse = false
                        });
                        Debug.WriteLine("Started Monitoring");
                        lblMessage.Text = "Started Monitoring";
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
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"ERROR: {ex.Message}");
                }

            });

        }

        public void NotifyMe()
        {
            lblMessage.Text = "ENTERED";
        }

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

        public ICommand Register { get; }
        public ICommand Stop { get; }
    }

}

