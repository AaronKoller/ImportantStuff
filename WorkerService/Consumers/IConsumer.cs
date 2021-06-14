using System.Threading.Tasks;
using WorkerService.Models;

namespace WorkerService.Consumers
{
    public interface IConsumer
    {
        Task Consume(Message message);
    }
}
