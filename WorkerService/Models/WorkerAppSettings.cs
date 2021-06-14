namespace WorkerService.Models
{
    public class WorkerAppSettings
    {
        public string ConnectionString { get; set; }
        public WorkerServiceSettings WorkerServiceSettings { get; set; }
    }

    public class WorkerServiceSettings
    {
        public int NumOfConsumers { get; set; }
    }
}
