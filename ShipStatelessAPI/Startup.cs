using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Owin;

namespace ShipStatelessAPI
{
    public class Startup
    {
        public static void ConfigureFormatters(MediaTypeFormatterCollection formatters)
        {
            formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            ConfigureFormatters(config.Formatters);
            appBuilder.UseWebApi(config);
        }
    }
}
