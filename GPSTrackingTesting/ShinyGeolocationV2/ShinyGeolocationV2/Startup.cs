using ShinyGeolocationV2.Models;
using Microsoft.Extensions.DependencyInjection;
using Shiny;
using Shiny.Logging;
namespace ShinyGeolocationV2
{
    public class Startup : ShinyStartup
    {
        public override void ConfigureServices(IServiceCollection builder)
        {
            // custom logging
            Log.UseConsole();
            Log.UseDebug();

            // create your infrastructure
            //builder.AddSingleton<SampleSqliteConnection>();

            //// register all of the acr stuff you want to use
            //builder.UseGpsBackground<>();
<<<<<<< HEAD
            builder.AddSingleton<AppNotifications>();
            builder.AddSingleton<CoreDelegateServices>();
            builder.UseGeofencing<MyGeofenceDelegate>();
            builder.UseNotifications(true);
=======
            builder.UseGeofencing<MyGeofenceDelegate>();
            builder.UseNotifications();
>>>>>>> GPSTrackingWithShiny
        }
    }
}
