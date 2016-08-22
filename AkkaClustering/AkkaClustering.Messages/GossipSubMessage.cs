using Akka.Actor;

namespace AkkaClustering.Messages
{
    public class GossipSubMessage
    {
        public IActorRef Actor { get; private set; }

        public GossipSubMessage(IActorRef actor)
        {
            this.Actor = actor;
        }
    }
}
