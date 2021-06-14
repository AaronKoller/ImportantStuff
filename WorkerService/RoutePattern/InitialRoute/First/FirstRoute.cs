using System;
using System.Collections.Generic;
using WorkerService.Models;
using WorkerService.RoutePattern.InitialRoute.First.Second;

namespace WorkerService.RoutePattern.InitialRoute.First
{
    public interface IFirstRoute : IRoute { }

    internal class FirstRoute : BasicRoute, IFirstRoute
    {
        public FirstRoute(IRoute unknownRoute) : base(unknownRoute)
        {
        }

        protected override Dictionary<string, IRoute> RouteTable { get; } = new Dictionary<string, IRoute>();
        protected override Func<Message, string> KeySelector { get; set; }

        public FirstRoute(ISecondRoute secondRoute, IFirstRouteUnknown firstRouteUnknown) : base(firstRouteUnknown)
        {
            KeySelector = message => message.Route.Dequeue();
            RouteTable.Add("second", secondRoute);
        }
    }
}
