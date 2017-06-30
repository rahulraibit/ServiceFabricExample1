using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using StatefulTrackerInterface;
using StatefulService = Microsoft.ServiceFabric.Services.Runtime.StatefulService;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Runtime;


namespace StatefulTracker
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class StatefulTracker : StatefulService, ILocationReporter, ILocationViewer
    {
        public StatefulTracker(StatefulServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]{
                new ServiceReplicaListener(
                    (context) => new FabricTransportServiceRemotingListener(context,this))};
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>

        public async Task ReportLocation(Location location)
        {
            using (var tx = StateManager.CreateTransaction())
            {
                var timestamps = await StateManager.GetOrAddAsync<IReliableDictionary<Guid, DateTime>>("timestamps");

                var timestamp = DateTime.UtcNow;

                // TODO: Update individual sheep actor

                // Update service with new timestamp
                await timestamps.AddOrUpdateAsync(tx, location.SheepId, DateTime.UtcNow, (guid, time) => timestamp);
                await tx.CommitAsync();
            }
        }

        public async Task<DateTime?> GetLastReportTime(Guid sheepId)
        {
            using (var tx = StateManager.CreateTransaction())
            {
                var timestamps = await StateManager.GetOrAddAsync<IReliableDictionary<Guid, DateTime>>("timestamps");

                var timestamp = await timestamps.TryGetValueAsync(tx, sheepId);
                await tx.CommitAsync();

                return timestamp.HasValue ? (DateTime?)timestamp.Value : null;
            }
        }

        public async Task<KeyValuePair<float, float>?> GetLastSheepLocation(Guid sheepId)
        {
            throw new NotImplementedException();
        }
    }
}
