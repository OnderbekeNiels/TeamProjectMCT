using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace TempoTrack.Droid
{
    [Activity(Label = "TempoTrack", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState); // for login with google
                                                                                                                       // User-Agent tweaks for Embedded WebViews
            global::Xamarin.Auth.WebViewConfiguration.Android.UserAgent = "Mozilla/5.0 (Linux; Android 4.4.4; One Build/KTU84L.H4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.135 Mobile Safari/537.36";

            #region Other Xamarin.Auth intialization properties

            // Xamarin.Auth CustomTabs Initialization/Customisation

            //global::Xamarin.Auth.CustomTabsConfiguration.ActionLabel = null;
            //global::Xamarin.Auth.CustomTabsConfiguration.MenuItemTitle = null;
            //global::Xamarin.Auth.CustomTabsConfiguration.AreAnimationsUsed = true;
            //global::Xamarin.Auth.CustomTabsConfiguration.IsShowTitleUsed = false;
            //global::Xamarin.Auth.CustomTabsConfiguration.IsUrlBarHidingUsed = false;
            //global::Xamarin.Auth.CustomTabsConfiguration.IsCloseButtonIconUsed = false;
            //global::Xamarin.Auth.CustomTabsConfiguration.IsActionButtonUsed = false;
            //global::Xamarin.Auth.CustomTabsConfiguration.IsActionBarToolbarIconUsed = false;
            //global::Xamarin.Auth.CustomTabsConfiguration.IsDefaultShareMenuItemUsed = false;
            //global::Xamarin.Auth.CustomTabsConfiguration.ToolbarColor = Android.Graphics.Color.LightBlue;

            //// ActivityFlags for tweaking closing of CustomTabs
            //// please report findings!
            //global::Xamarin.Auth.CustomTabsConfiguration.
            //   ActivityFlags =
            //        global::Android.Content.ActivityFlags.NoHistory
            //        |
            //        global::Android.Content.ActivityFlags.SingleTop
            //        |
            //        global::Android.Content.ActivityFlags.NewTask;
            //global::Xamarin.Auth.CustomTabsConfiguration.IsWarmUpUsed = true;
            //global::Xamarin.Auth.CustomTabsConfiguration.IsPrefetchUsed = true;

            #endregion

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}