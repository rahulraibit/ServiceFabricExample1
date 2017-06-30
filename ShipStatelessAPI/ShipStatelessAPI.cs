using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Description;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Owin;

namespace ShipStatelessAPI
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class ShipStatelessAPI : StatelessService
    {
        public ShipStatelessAPI(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            //return new ServiceInstanceListener[0];
            var endpoints =
                Context.CodePackageActivationContext.GetEndpoints()
                    .Where(
                        endpoint =>
                            endpoint.Protocol == EndpointProtocol.Http || endpoint.Protocol == EndpointProtocol.Https)
                    .Select(endpoint => endpoint.Name);
            return
                endpoints.Select(
                    endpoint =>
                        new ServiceInstanceListener(initParams =>
                                new OwinCommunicationListener( "api", appbuilder => new Startup().Configuration(appbuilder), initParams), endpoint));
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>

    }
}
