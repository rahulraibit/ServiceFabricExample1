using System;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace StatefulTrackerInterface
{
    public static class TrackerConnectionFactory
    {
        private static readonly Uri LocationReporterServiceUrl = new Uri("fabric:/FirstServiceFabricWebAPI/StatefulTracker");

        public static ILocationReporter CreateLocationReporter()
        {
            var partition = new ServicePartitionKey(1);
            return ServiceProxy.Create<ILocationReporter>(LocationReporterServiceUrl, partition);
        }
        public static ILocationViewer CreateLocationViewer()
        {
            var partition = new ServicePartitionKey(2);
            return ServiceProxy.Create<ILocationViewer>(LocationReporterServiceUrl, partition);
        }
    }
}
