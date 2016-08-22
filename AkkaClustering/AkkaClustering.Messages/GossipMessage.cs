using Akka.Actor;

namespace AkkaClustering.Messages
{
    public class GossipMessage
    {
        public string Message { get; private set; }

        public GossipMessage(string message)
        {
            this.Message = message;
        }
    }
}
