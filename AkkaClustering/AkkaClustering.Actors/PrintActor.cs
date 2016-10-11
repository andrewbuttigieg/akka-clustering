using Akka.Actor;
using AkkaClustering.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaClustering.Actors
{
    public class PrintActor: ReceiveActor
    {
        public PrintActor()
        {
            Receive<ClusterMessage>(msg =>
            {
                Console.WriteLine("[" + DateTime.Now + "] " + msg.Message);
            });
        }
    }
}
