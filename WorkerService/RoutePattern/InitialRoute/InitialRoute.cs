using System;
using System.Collections.Generic;
using WorkerService.Models;
using WorkerService.RoutePattern.InitialRoute.First;

namespace WorkerService.RoutePattern.InitialRoute
{
    public interface IInitialRoute : IRoute { }

    internal class InitialRoute : BasicRoute, IInitialRoute
    {
        public InitialRoute(IRoute unknownRoute) : base(unknownRoute)
        {
        }

        protected override Dictionary<string, IRoute> RouteTable { get; } = new Dictionary<string, IRoute>();
        protected override Func<Message, string> KeySelector { get; set; }

        public InitialRoute(IFirstRoute firstRoute, IInitialRouteUnknown initialRouteUnknown) : base(initialRouteUnknown)
        {
            KeySelector = message => message.Route.Dequeue();
            RouteTable.Add("first", firstRoute);
        }
    }
}
