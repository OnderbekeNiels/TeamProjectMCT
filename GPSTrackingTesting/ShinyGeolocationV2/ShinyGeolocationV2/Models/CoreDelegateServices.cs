using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Shiny;
using Shiny.Notifications;
using ShinyGeolocationV2.Models;


namespace ShinyGeolocationV2.Models
{
    public class CoreDelegateServices
    {
        public CoreDelegateServices(AppNotifications notifications)
        {
            this.Notifications = notifications;
        }
        public AppNotifications Notifications { get; }
    }
}
