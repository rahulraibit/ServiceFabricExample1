
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace StatefulTrackerInterface
{
    public interface ILocationReporter : IService
    {
        Task ReportLocation(Location location);
    }
}
