using GeofencingShinyFramework.Models;
using Microsoft.Extensions.DependencyInjection;
using Shiny;
using Shiny.Logging;
namespace GeofencingShinyFramework
{
    public class Startup : ShinyStartup
    {
        public override void ConfigureServices(IServiceCollection builder)
        {
            // custom logging
            Log.UseConsole();
            Log.UseDebug();

            // create your infrastructure
            builder.AddSingleton<SampleSqliteConnection>();

            // register all of the acr stuff you want to use
            builder.UseGpsBackground<>();
            builder.UseGeofencing<MyGeofenceDelegate>();
            builder.UseNotifications();
        }
    }
}
