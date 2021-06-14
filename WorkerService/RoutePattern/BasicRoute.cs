using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkerService.Models;

namespace WorkerService.RoutePattern
{
    public interface IRoute
    {
        Task MoveNext(Message message);
    }

    internal abstract class BasicRoute : IRoute
    {
        private readonly IRoute _unknownRoute;

        protected abstract Dictionary<string, IRoute> RouteTable { get; }
        protected abstract Func<Message, string> KeySelector { get; set; }

        protected BasicRoute(IRoute unknownRoute)
        {
            _unknownRoute = unknownRoute;
        }

        public Task MoveNext(Message message)
        {
            var cursorStop = KeySelector.Invoke(message);
            var foundNextConsumer = RouteTable.TryGetValue(cursorStop, out var nextRoute);
            if (!foundNextConsumer)
            {
                return _unknownRoute.MoveNext(message);
            }

            return nextRoute.MoveNext(message);
        }
    }
}
