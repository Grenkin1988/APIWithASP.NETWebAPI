using Microsoft.Web.Http;
using Microsoft.Web.Http.Versioning;
using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace TheCodeCamp {
    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {
            // Web API configuration and services
            AutofacConfig.Register();

            config.AddApiVersioning(cnf => {
                cnf.DefaultApiVersion = new ApiVersion(1, 1);
                cnf.AssumeDefaultVersionWhenUnspecified = true;
                cnf.ReportApiVersions = true;
                cnf.ApiVersionReader = new HeaderApiVersionReader("X-Version");
            });

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}
