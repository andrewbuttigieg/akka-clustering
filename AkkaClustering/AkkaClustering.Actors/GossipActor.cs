using Akka.Actor;
using AkkaClustering.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaClustering.Actors
{
    public class GossipActor : ReceiveActor
    {
        private HashSet<IActorRef> subs = new HashSet<IActorRef>();

        public GossipActor()
        {
            Receive<GossipSubMessage>(msg =>
            {
                if (subs.Equals(msg.Actor) == false)
                    subs.Add(msg.Actor);
            });

            Receive<GossipUnSubMessage>(msg =>
            {
                if (subs.Equals(msg.Actor) == true)
                    subs.Remove(msg.Actor);
            });

            Receive<GossipMessage>(msg =>
            {
                foreach (var sub in subs)
                    sub.Tell(msg);
            });
        }
    }
}
