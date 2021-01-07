using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using TeamProject_LocationTrigger.Models;

namespace TeamProject_LocationTrigger.Droid
{
    [Activity(Label = "TeamProject_LocationTrigger", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        public static Context AppContext;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            //Set the app context for the geofencing
            AppContext = this.ApplicationContext;
            //TODO: Initialize CrossGeofence Plugin
            //TODO: Specify the listener class implementing IGeofenceListener interface in the Initialize generic
            //CrossGeofence.Initialize<CrossGeofenceListener>();
            //CrossGeofence.GeofenceListener.OnAppStarted();
            //Start a sticky service to keep receiving geofence events when app is closed.
            // URL to this Plugin's Documentation: https://github.com/CrossGeeks/GeofencePlugin
            StartService();
        }

        /// <summary>
        /// Function that is used by geofencing plugin to start the geofencing service for android
        /// </summary>
        public static void StartService()
        {
            AppContext.StartService(new Intent(AppContext, typeof(GeofenceService)));

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
            {

                PendingIntent pintent = PendingIntent.GetService(AppContext, 0, new Intent(AppContext, typeof(GeofenceService)), 0);
                AlarmManager alarm = (AlarmManager)AppContext.GetSystemService(Context.AlarmService);
                alarm.Cancel(pintent);
            }
        }

        /// <summary>
        /// Function that is used to stop geofencing on android platform
        /// </summary>
        public static void StopService()
        {
            AppContext.StopService(new Intent(AppContext, typeof(GeofenceService)));
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
            {
                PendingIntent pintent = PendingIntent.GetService(AppContext, 0, new Intent(AppContext, typeof(GeofenceService)), 0);
                AlarmManager alarm = (AlarmManager)AppContext.GetSystemService(Context.AlarmService);
                alarm.Cancel(pintent);
            }
        }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}