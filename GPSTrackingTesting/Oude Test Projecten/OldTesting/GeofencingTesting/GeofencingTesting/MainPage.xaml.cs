using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GeofencingTesting
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        //LocationManager
        PendingIntent pendingIntent = ...; //Intent to launch
        LatLng mpCoord = ...; //Center point
        float alertRadius ... ; //Radius of proximity in meters
        long expiration = -1; //Timeout in msec, -1 = infinite

        locManager.AddProximityAlert(mpCoord.Latitude, mpCoord.Longtitude, alertRadius, timeout, pendingIntent);


    }
}
