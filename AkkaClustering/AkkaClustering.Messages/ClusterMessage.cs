using Akka.Actor;

namespace AkkaClustering.Messages
{
    public class ClusterMessage
    {
        public string Message { get; private set; }

        public ClusterMessage(string message)
        {
            this.Message = message;
        }
    }
}
