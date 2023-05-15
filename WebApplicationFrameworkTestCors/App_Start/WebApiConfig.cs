using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Routing;

namespace WebApplicationFrameworkTestCors
{
    public static class WebApiConfig
    {

        public class GuidConstraint : System.Web.Routing.IRouteConstraint
        {

            public bool Match(System.Web.HttpContextBase httpContext, System.Web.Routing.Route route, string parameterName, System.Web.Routing.RouteValueDictionary values, System.Web.Routing.RouteDirection routeDirection)
            {
                if (values.ContainsKey(parameterName))
                {
                    string stringValue = values[parameterName] as string;

                    if (!string.IsNullOrEmpty(stringValue))
                    {
                        Guid guidValue;

                        return Guid.TryParse(stringValue, out guidValue) && (guidValue != Guid.Empty);
                    }
                }

                return false;
            }
        }

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //var cors = new EnableCorsAttribute("https://xwc-uat-nbc.fnis.com.br", "*", "*");
           // var cors = new EnableCorsAttribute("*", "*", "*");
            //config.EnableCors(cors);

            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //config.Routes.MapHttpRoute("DefaultApiWithId", "Api/{controller}/{id}", new { id = RouteParameter.Optional }, new { id = new GuidConstraint() });
            //config.Routes.MapHttpRoute("DefaultApiWithAction", "Api/{controller}/{action}");
            //config.Routes.MapHttpRoute("DefaultApiGet", "Api/{controller}", new { action = "Get" }, new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });
            //config.Routes.MapHttpRoute("DefaultApiPost", "Api/{controller}", new { action = "Post" }, new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) });
            //config.Routes.MapHttpRoute("DefaultApiOptions", "Api/{controller}", new { action = "Options" }, new { httpMethod = new HttpMethodConstraint(HttpMethod.Options) });

        }
    }
}
