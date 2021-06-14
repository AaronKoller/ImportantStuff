using System.Threading.Tasks;

namespace WorkerService.Messaging
{

    public interface IProducer
    {
        Task Send(string message);
    }

    public class Producer : IProducer
    {
        public async Task Send(string message)
        {
            await Task.Delay(10);
        }
    }
}
