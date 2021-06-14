using System.Threading.Tasks;
using WorkerService.Messaging;
using WorkerService.Models;

namespace WorkerService.Consumers
{
    internal interface IKnownMessageConsumer : IConsumer { }
    internal class KnownMessageConsumer : IKnownMessageConsumer
    {
        private readonly IProducer _producer;

        public KnownMessageConsumer(IProducer producer)
        {
            _producer = producer;
        }
        public async Task Consume(Message message)
        {
            await _producer.Send(message.Body);
        }
    }
}
