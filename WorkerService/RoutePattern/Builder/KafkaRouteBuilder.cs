using System.Collections.Generic;
using WorkerService.Models;

namespace WorkerService.RoutePattern.Builder
{
    public interface IRouteBuilder
    {
        Queue<string> Build(Message message);
    }


    public class KafkaRouteBuilder : IRouteBuilder
    {
        public Queue<string> Build(Message message)
        {
            var headers = message.Headers;

            headers.TryGetValue(WorkserService_Constants.Topic, out var topic);

            var lowerTopic = topic.ToString().ToLower();

            var queue = new Queue<string>(lowerTopic.Split('.'));

            return queue;

            //return new Queue<string>(message.Headers[WorkserService_Constants.Topic].ToString().ToLower().Split("."));
        }
    }
}
