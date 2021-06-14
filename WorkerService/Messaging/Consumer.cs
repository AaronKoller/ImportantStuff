using System.Collections.Generic;
using WorkerService.Models;

namespace WorkerService.Messaging
{
    public interface IConsumer
    {
        public Message Get();
    }

    public class Consumer : IConsumer
    {
        public Message Get()
        {
            return new Message
            {
                Body = "\"Property\": \"Value\"",
                Headers = new Dictionary<string, object>
                {
                    {
                        WorkserService_Constants.Topic, "First.Second.Third"
                    }
                }
            };
        }
    }
}
