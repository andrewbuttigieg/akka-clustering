using Akka.Actor;
using Akka.Configuration;
using AkkaClustering.Actors;
using AkkaClustering.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaClustering.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            string seedConfig = @"
akka {
    actor {
        provider = ""Akka.Cluster.ClusterActorRefProvider, Akka.Cluster""
    }
    remote {
        helios.tcp {
            transport-class = ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
            applied-adapters = []
            transport-protocol = tcp
            port = 50003
            hostname = localhost
        }
    }
}
";

            var config = ConfigurationFactory.ParseString(seedConfig);

            using (ActorSystem system = ActorSystem.Create("ClusterSystem", config))
            {
                var gossipActor = system.ActorOf(Props.Create(() => new GossipActor()), "gossipActor");


                var someMessage = new GossipMessage("This is a message.");
                system
                   .Scheduler
                   .Schedule(TimeSpan.FromSeconds(0),
                             TimeSpan.FromSeconds(5),
                             gossipActor, someMessage);

                Console.ReadKey();
            }
        }
    }
}