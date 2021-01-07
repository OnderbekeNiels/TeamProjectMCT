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
using static Accounts.ACAccountStore;

namespace GeofencingShinyFramework
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // shiny doesn't usually manage your viewmodels, so we'll do this for now
            var geofences = ShinyHost.Resolve<IGeofenceManager>();
            var notifications = ShinyHost.Resolve<INotificationManager>();


            //StartGeofencingAsync();
            Register = new Command(async () =>
            {

                Debug.WriteLine("Starting Geofencing");
                // this is really only required on iOS, but do it to be safe
                var access = await notifications.RequestAccess();
                try {
                    if (access == AccessState.Available)
                    {
                        await geofences.StartMonitoring(new GeofenceRegion(
                            "CN Tower - Toronto, Canada",
                            new Position(50.915124678620415, 3.651215601701313),
                            Distance.FromMeters(200))
                        {
                            NotifyOnEntry = true,
                            NotifyOnExit = true,
                            SingleUse = false
                        });
                    }
                }
                catch(Exception ex)
                {
                    Debug.WriteLine($"ERROR: {ex.Message}");
                }
                
            });

        

            
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

        protected override void OnAppearing()
        {
            //Toestemming vragen
            AskPermitionsAsync();
        }

        //public async Task StartGeofencingAsync()
        //{
        //    bool access = true;
        //    if (access)
        //    {
        //        await this.geofenceManager.StartMonitoring(new GeofenceRegion(
        //               "Home",
        //               new Position(50.915124678620415, 3.651215601701313),
        //               Distance.FromMeters(100)
        //           )
        //        {
        //            NotifyOnEntry = true,
        //            NotifyOnExit = true,
        //            SingleUse = false
        //        });
        //    }
        //}

        public ICommand Register { get; }
    }

}

