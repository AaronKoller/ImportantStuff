namespace ConstructorChainingInjection
{
    public interface IWorkerService
    {
        void DoWork();
    }

    public class WorkerService : IWorkerService
    {
        private readonly IWorkerProvider _workerProvider;
        private readonly IThingProvider _thingProvider;
        private readonly ISnarfProvider _snarfProvider;

        //"Default" Constructor
        public WorkerService() : this(new WorkerProvider(), new ThingProvider(), new SnarfProvider()) { }

        //"Secondary" Constructor 1
        public WorkerService(IThingProvider thingProvider) : this(new WorkerProvider(), thingProvider, new SnarfProvider()) { }

        //"Secondary" Constructor 2
        public WorkerService(IThingProvider thingProvider, IWorkerProvider workerProvider) : this(workerProvider, thingProvider, new SnarfProvider()) { }

        //"Primary" Constructor
        public WorkerService(IWorkerProvider workerProvider, IThingProvider thingProvider, ISnarfProvider snarfProvider)
        {
            _workerProvider = workerProvider;
            _thingProvider = thingProvider;
            _snarfProvider = snarfProvider;
        }

        public void DoWork()
        {
            
        }
    }
}