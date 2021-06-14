using System.Threading.Tasks;
using WorkerService.Consumers;
using WorkerService.Models;

namespace WorkerService.RoutePattern.InitialRoute.First.Second.Third
{
    public interface IThirdRoute : IRoute { }

    internal class ThirdRoute : IThirdRoute
    {
        private readonly IKnownMessageConsumer _knownMessageConsumer;
        private readonly IUnknownMessageConsumer _unknownMessageConsumer;


        public ThirdRoute(IKnownMessageConsumer knownMessageConsumer, IUnknownMessageConsumer unknownMessageConsumer)
        {
            _knownMessageConsumer = knownMessageConsumer;
            _unknownMessageConsumer = unknownMessageConsumer;
        }

        public async Task MoveNext(Message message)
        {
            if (string.IsNullOrWhiteSpace(message.Body))
            {
                await _unknownMessageConsumer.Consume(message);
            }

            await _knownMessageConsumer.Consume(message);
        }
    }
}
