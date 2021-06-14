using System.Threading.Tasks;
using WorkerService.Consumers;
using WorkerService.Models;
using WorkerService.Models.Exceptions;

namespace WorkerService.RoutePattern.InitialRoute.First
{
    public interface IFirstRouteUnknown : IRoute { }

    internal class FirstRouteUnknown : IFirstRouteUnknown
    {
        private readonly IUnknownMessageConsumer _unknownMessageConsumer;

        public FirstRouteUnknown(IUnknownMessageConsumer unknownMessageConsumer)
        {
            _unknownMessageConsumer = unknownMessageConsumer;
        }

        public async Task MoveNext(Message message)
        {
            await _unknownMessageConsumer.Consume(message);
            throw new RouteNotFoundException((string)message.Headers[WorkserService_Constants.Topic]);
        }
    }
}
