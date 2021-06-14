using System.Threading.Tasks;
using WorkerService.Models;

namespace WorkerService.Consumers
{
    public interface IUnknownMessageConsumer : IConsumer { }
    internal class UnknownMessageConsumer : IUnknownMessageConsumer
    {
        public async Task Consume(Message message)
        {
            await Task.Delay(10);
        }
    }
}
