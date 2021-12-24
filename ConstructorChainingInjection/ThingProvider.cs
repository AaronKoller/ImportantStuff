namespace ConstructorChainingInjection
{
    public interface IThingProvider
    {
    }

    public class ThingProvider : IThingProvider
    {
        private readonly ISnarfProvider _snarfProvider;

        //"Default" Constructor
        public ThingProvider() : this(new SnarfProvider()) { }

        //"Primary" Constructor
        public ThingProvider(ISnarfProvider snarfProvider)
        {
            _snarfProvider = snarfProvider;
        }
    }
}