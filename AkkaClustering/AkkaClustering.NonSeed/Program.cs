using Akka.Actor;
using Akka.Configuration;
using AkkaClustering.Actors;
using AkkaClustering.Messages;
using System;

namespace AkkaClustering.NonSeed
{
    class Program
    {
        static void Main(string[] args)
        {
            var nonSeedConfig = @"
akka {
    actor.provider = ""Akka.Cluster.ClusterActorRefProvider, Akka.Cluster""
    remote {
        helios.tcp {
            transport-class = ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
            applied-adapters = []
            transport-protocol = tcp
            port = 0
            hostname = localhost
        }
    }
    deployment {
        /user/gossipActor {
	        cluster {
			        enabled = on
			        max-nr-of-instances-per-node = 1
			        allow-local-routees = on
			        use-role = tracker
	        }
        }
    }
    cluster {
        seed-nodes = [""akka.tcp://ClusterSystem@localhost:50003""]
    }
}";

            var config = ConfigurationFactory.ParseString(nonSeedConfig);

            using (ActorSystem system = ActorSystem.Create("ClusterSystem", config))
            {
                //var gossipActor = system.ActorOf(Props.Create(() => new GossipActor()), "gossipActor");
                var gossipActor = system.ActorSelection("akka.tcp://ClusterSystem@localhost:50003/user/gossipActor");
                var roomActor = system.ActorSelection("akka.tcp://ClusterSystem@localhost:50003/user/roomActor");
                //var roomActor = system.ActorOf(Props.Create(() => new RoomActor()), "roomActor");
                var printActor = system.ActorOf(Props.Create(() => new PrintActor()), "printActor");

                gossipActor.Tell(new GossipSubMessage(printActor));

                var someMessage = new GossipMessage("This is a message from the non seed.");
                system
                   .Scheduler
                   .ScheduleTellRepeatedly(TimeSpan.FromSeconds(3),
                             TimeSpan.FromSeconds(5),
                             gossipActor, someMessage, roomActor.Anchor);
                

                Console.ReadLine();
            }
        }
    }
}
