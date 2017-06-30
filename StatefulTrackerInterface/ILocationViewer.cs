using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;


namespace StatefulTrackerInterface
{
    public interface ILocationViewer : IService
    {
        Task<KeyValuePair<float, float>?> GetLastSheepLocation(Guid sheepId);
        Task<DateTime?> GetLastReportTime(Guid sheepId);
    }
}
