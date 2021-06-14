using System.Threading.Tasks;
using WorkerService.Consumers;
using WorkerService.Models;
using WorkerService.Models.Exceptions;

namespace WorkerService.RoutePattern.InitialRoute
{
    public interface IInitialRouteUnknown : IRoute { }

    internal class InitialRouteUnknown : IInitialRouteUnknown
    {
        private readonly IUnknownMessageConsumer _unknownMessageConsumer;

        public InitialRouteUnknown(IUnknownMessageConsumer unknownMessageConsumer)
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
