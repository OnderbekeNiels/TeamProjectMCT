using Microsoft.Extensions.DependencyInjection;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeofencingShinyFramework.Models
{
    public class SampleStartup : ShinyStartup
    {
        public override void ConfigureServices(IServiceCollection builder)
        {
            builder.UseGeofencing<MyGeofenceDelegate>();
            builder.UseNotifications(true);
        }
    }
}
