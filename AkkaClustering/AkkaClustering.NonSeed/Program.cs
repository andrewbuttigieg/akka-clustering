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
            port = 0 #let os pick random port
            hostname = localhost
        }
    }
    cluster {
        seed-nodes = [""akka.tcp://ClusterSystem@localhost:50003""]
    }
}";

            var config = ConfigurationFactory.ParseString(nonSeedConfig);

            using (ActorSystem system = ActorSystem.Create("ClusterSystem", config))
            {
                var gossipActor = system.ActorOf<GossipActor>("gossipActor");
                var printActor = system.ActorOf(Props.Create(() => new PrintActor()), "printActor");

                gossipActor.Tell(new GossipSubMessage(printActor));

                Console.ReadLine();
            }
        }
    }
}
