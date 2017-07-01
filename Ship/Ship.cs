using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using Ship.Interfaces;

namespace Ship
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    public sealed class Ship : Actor, IShip
    {
        /// <summary>
        /// Initializes a new instance of ShipActors
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public Ship(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        [DataContract]
        internal sealed class LocationAtTime
        {
            public DateTime Timestamp { get; set; }
            public float Latitude { get; set; }
            public float Longitude { get; set; }
        }

        [DataContract]
        internal sealed class SheepState
        {
            [DataMember]
            public List<LocationAtTime> LocationHistory { get; set; }
        }

        protected override async Task OnActivateAsync()
        {
            var state = await StateManager.TryGetStateAsync<SheepState>("State");
            if (!state.HasValue)
                await StateManager.AddStateAsync("State", new SheepState { LocationHistory = new List<LocationAtTime>() });
        }

        public async Task SetLocation(DateTime timestamp, float latitude, float longitude)
        {
            var state = await StateManager.GetStateAsync<SheepState>("State");
            state.LocationHistory.Add(new LocationAtTime() { Timestamp = timestamp, Latitude = latitude, Longitude = longitude });
            await StateManager.AddOrUpdateStateAsync("State", state, (s, actorState) => state);
        }

        public async Task<KeyValuePair<float, float>> GetLatestLocation()
        {
            var state = await StateManager.GetStateAsync<SheepState>("State");
            var location = state.LocationHistory.OrderByDescending(x => x.Timestamp).Select(x =>
                new KeyValuePair<float, float>(x.Latitude, x.Longitude)
            ).FirstOrDefault();
            return location;
        }
    }
}
