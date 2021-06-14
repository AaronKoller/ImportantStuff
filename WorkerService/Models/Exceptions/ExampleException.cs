using System;
using System.Runtime.Serialization;

namespace WorkerService.Models.Exceptions
{
    [Serializable]
    public sealed class RouteNotFoundException : Exception
    {
        public RouteNotFoundException(string message) : base(message) { }
        public RouteNotFoundException(string message, Exception innerException) : base(message, innerException) { }
        private RouteNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
