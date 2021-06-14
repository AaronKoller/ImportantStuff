using System.Threading.Tasks;

namespace WorkerService.Mappers
{
    public interface IMapper<TInput, TOutput>
    {
        Task<TOutput> Map(TInput input);
    }
}
