using Akka.Actor;
using Akka.Configuration;
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
            string seedConfig = @"akka {
    actor.provider = ""Akka.Cluster.ClusterActorRefProvider, Akka.Cluster""
    remote {
                helios.tcp {
                    port = 8081
                    hostname = localhost
                }
            }
            cluster {
                seed-nodes = [""akka.tcp://ClusterSystem@127.0.0.1:8081""]
            }
        }";

            var config = ConfigurationFactory.ParseString(seedConfig);

            using (ActorSystem system = ActorSystem.Create("ClusterSystem", config))
            {
                //var chatHistory = system.ActorOf<ChatHistoryActor>("chatHistory");
                //system.ActorOf(Props.Create(() => new ChatWriterActor(chatHistory)), "chatWriter");

                Console.ReadKey();
            }

        }
    }
}
