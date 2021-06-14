using System.Threading.Tasks;

namespace WorkerService.Mappers
{
    public class StringMapper : IMapper<int, string>
    {
        public async Task<string> Map(int input)
        {
            return input.ToString();
        }
    }
}
