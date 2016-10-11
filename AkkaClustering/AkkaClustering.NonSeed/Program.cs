using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
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
    actor {
        provider = ""Akka.Cluster.ClusterActorRefProvider, Akka.Cluster""
        deployment {
            /talker {
                router = broadcast-group
                routees.paths = [""/user/printActor""]
                nr-of-instances = 3
                cluster {
			        enabled = on
					allow-local-routees = on
					use-role = tracker
	            }
            }
        }
    }
    remote {
        helios.tcp {
            transport-class = ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
            applied-adapters = []
            transport-protocol = tcp
            port = 0
            hostname = localhost
        }
    }
    cluster {
        seed-nodes = [""akka.tcp://clusterSystem@localhost:50003""]
        roles = [""tracker""]
    }
}";

            var config = ConfigurationFactory.ParseString(nonSeedConfig);

            using (ActorSystem system = ActorSystem.Create("clusterSystem", config))
            {
                var printActor = system.ActorOf(Props.Create(() => new PrintActor()), "printActor");
                var router = system.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "talker");

                var someMessage = new ClusterMessage("This is a message from the non seed.");

                system
                   .Scheduler
                   .ScheduleTellRepeatedly(TimeSpan.FromSeconds(0),
                             TimeSpan.FromSeconds(5),
                             router, someMessage, ActorRefs.NoSender);
                
                Console.ReadLine();
            }
        }
    }
}
