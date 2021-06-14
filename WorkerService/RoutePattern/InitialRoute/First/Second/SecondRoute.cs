using System;
using System.Collections.Generic;
using WorkerService.Models;
using WorkerService.RoutePattern.InitialRoute.First.Second.Third;

namespace WorkerService.RoutePattern.InitialRoute.First.Second
{
    public interface ISecondRoute : IRoute { }

    internal class SecondRoute : BasicRoute, ISecondRoute
    {
        public SecondRoute(IRoute unknownRoute) : base(unknownRoute)
        {
        }

        protected override Dictionary<string, IRoute> RouteTable { get; } = new Dictionary<string, IRoute>();
        protected override Func<Message, string> KeySelector { get; set; }

        public SecondRoute(IThirdRoute secondRoute, ISecondRouteUnknown secondRouteUnknown) : base(secondRouteUnknown)
        {
            KeySelector = message => message.Route.Dequeue();
            RouteTable.Add("third", secondRoute);
        }
    }
}
