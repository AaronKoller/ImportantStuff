using System;
using System.Runtime.Serialization;

namespace Api.Models.Exceptions
{
    [Serializable]
    public sealed class ExampleException : Exception
    {
        public ExampleException(string message) : base(message) { }
        public ExampleException(string message, Exception innerException) : base(message, innerException) { }
        private ExampleException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
