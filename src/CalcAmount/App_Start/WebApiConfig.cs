using Microsoft.Web.Http;
using System.Web.Http;

public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        //config.AddApiVersioning(o =>
        //{
        //    o.AssumeDefaultVersionWhenUnspecified = true;
        //    o.DefaultApiVersion = new ApiVersion(1, 0);
        //    o.ReportApiVersions = true;
        //});

        // Attribute routing.
        config.MapHttpAttributeRoutes();

        // Convention-based routing.
        config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
        );
    }
}
