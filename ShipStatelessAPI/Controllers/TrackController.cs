﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using StatefulTrackerInterface;

namespace ShipStatelessAPI.Controllers
{
    public class TrackController : ApiController
    {
        [HttpGet]
        [Route("")]
        public string Index()
        {
            return "Welcome to Sheepishly 1.0.0 - The Combleat Sheep Tracking Suite";
        }

        [HttpPost]
        [Route("locations")]
        public async Task<bool> Log(Location location)
        {
            var reporter = TrackerConnectionFactory.CreateLocationReporter();
            await reporter.ReportLocation(location);
            return true;
        }

        [HttpGet]
        [Route("sheep/{sheepId}/lastseen")]
        public async Task<DateTime?> LastSeen(Guid sheepId)
        {
            var viewer = TrackerConnectionFactory.CreateLocationViewer();
            return await viewer.GetLastReportTime(sheepId);
        }

        [HttpGet]
        [Route("sheep/{sheepId}/lastlocation")]
        public async Task<object> LastLocation(Guid sheepId)
        {
            var viewer = TrackerConnectionFactory.CreateLocationViewer();
            var location = await viewer.GetLastSheepLocation(sheepId);
            if (location == null)
                return null;

            return new
            {
                Latitude = location.Value.Key,
                Longitude = location.Value.Value
            };
        }
    }
}
