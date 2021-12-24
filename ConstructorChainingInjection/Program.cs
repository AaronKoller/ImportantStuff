namespace ConstructorChainingInjection
{
    class Program
    {
        static void Main(string[] args)
        {
            IWorkerService workerService = new WorkerService();
            workerService.DoWork();
        }
    }
}
