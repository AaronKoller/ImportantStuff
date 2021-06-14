using System.Threading.Tasks;
using WorkerService.Consumers;
using WorkerService.Models;
using WorkerService.Models.Exceptions;

namespace WorkerService.RoutePattern.InitialRoute.First.Second
{
    public interface ISecondRouteUnknown : IRoute { }

    internal class SecondRouteUnknown : ISecondRouteUnknown
    {
        private readonly IUnknownMessageConsumer _unknownMessageConsumer;

        public SecondRouteUnknown(IUnknownMessageConsumer unknownMessageConsumer)
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
