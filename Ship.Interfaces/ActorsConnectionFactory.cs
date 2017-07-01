using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace Ship.Interfaces
{
    public static class ActorsConnectionFactory
    {
        private static readonly Uri SheepServiceUrl = new Uri("fabric:/FirstServiceFabricWebAPI/ShipActorService");

        public static IShip GetSheep(ActorId actorId)
        {
            return ActorProxy.Create<IShip>(actorId, SheepServiceUrl);
        }
    }
}
