using System.Collections.Generic;

namespace WorkerService.Models
{
    public class Message
    {
        public string Body { get; set; }
        public Dictionary<string, object> Headers { get; set; } = new Dictionary<string, object>();
        public Queue<string> Route { get; set; }
    }
}
