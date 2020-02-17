using Microsoft.Web.Http;
using Microsoft.Web.Http.Routing;
using Microsoft.Web.Http.Versioning;
using Microsoft.Web.Http.Versioning.Conventions;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
using System.Web.Http.Routing;

namespace TheCodeCamp {
    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {
            // Web API configuration and services
            AutofacConfig.Register();

            config.AddApiVersioning(cnf => {
                cnf.DefaultApiVersion = new ApiVersion(1, 1);
                cnf.AssumeDefaultVersionWhenUnspecified = true;
                cnf.ReportApiVersions = true;
                cnf.ApiVersionReader =
                    //new UrlSegmentApiVersionReader();
                    ApiVersionReader.Combine( 
                        new HeaderApiVersionReader("X-Version"),
                        new QueryStringApiVersionReader("ver"));
            });

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            //var constraintResolver = new DefaultInlineConstraintResolver { 
            //    ConstraintMap = {
            //        ["apiVersion"] = typeof(ApiVersionRouteConstraint)
            //    }
            //};

            // Web API routes
            //config.MapHttpAttributeRoutes(constraintResolver);
            config.MapHttpAttributeRoutes();
        }
    }
}
