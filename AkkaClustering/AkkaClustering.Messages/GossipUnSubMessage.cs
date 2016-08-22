using Akka.Actor;

namespace AkkaClustering.Messages
{
    public class GossipUnSubMessage
    {
        public IActorRef Actor { get; private set; }

        public GossipUnSubMessage(IActorRef actor)
        {
            this.Actor = actor;
        }
    }
}
