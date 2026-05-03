using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomReservationSystem.Shared.DTOs.ApiDescription;
using System.Reflection;

namespace RoomReservationSystem.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiDescriptorController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get()
        {
            List<ApiEndpointDto> endpoints = new List<ApiEndpointDto>();
            var assembly = Assembly.GetExecutingAssembly();
            var controllers = assembly.GetTypes().Where(t => typeof(ControllerBase).IsAssignableFrom(t));

            foreach (var controller in controllers)
            {
                var controllerRoute = controller.GetCustomAttribute<RouteAttribute>()?.Template ?? string.Empty;

                var methods = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance |
                    BindingFlags.DeclaredOnly);
                foreach (var method in methods)
                {
                    string httpMethod = "UNKNOWN";
                    string methodRoute = string.Empty;
                    if (method.GetCustomAttribute<HttpGetAttribute>() is { } httpGet)
                    {
                        httpMethod = "GET";
                        methodRoute = httpGet.Template ?? string.Empty;
                    }
                    else if (method.GetCustomAttribute<HttpPostAttribute>() is { } httpPost)
                    {
                        httpMethod = "POST";
                        methodRoute = httpPost.Template ?? string.Empty;
                    }
                    else if (method.GetCustomAttribute<HttpPutAttribute>() is { } httpPut)
                    {
                        httpMethod = "PUT";
                        methodRoute = httpPut.Template ?? string.Empty;
                    }
                    else if (method.GetCustomAttribute<HttpDeleteAttribute>() is { } httpDelete)
                    {
                        httpMethod = "DELETE";
                        methodRoute = httpDelete.Template ?? string.Empty;
                    }
                    else { continue; }

                    var fullRoute = string.IsNullOrEmpty(methodRoute) ? controllerRoute :
                        $"{controllerRoute}/{methodRoute}";

                    var parameters = method.GetParameters().Select(p => new ApiParameterDto()
                    {
                        Name = p.Name ?? string.Empty,
                        Type = p.ParameterType.Name,
                        Source = (p.GetCustomAttribute<FromBodyAttribute>() is not null) ?
                            "Body" : (p.GetCustomAttribute<FromQueryAttribute>() is not null) ?
                            "Query" : "Route"
                    }).ToList();

                    bool requiresAuth = method.GetCustomAttribute<AuthorizeAttribute>() is not null ||
                        controller.GetCustomAttribute<AuthorizeAttribute>() is not null;

                    endpoints.Add(new ApiEndpointDto()
                    {
                        Controller = controller.Name.Replace("Controller", string.Empty),
                        Action = method.Name,
                        HttpMethod = httpMethod,
                        Route = fullRoute,
                        Parameters = parameters,
                        Returns = method.ReturnType.Name,
                        RequiresAuth = requiresAuth
                    });
                }
            }

            return Ok(endpoints);
        }
    }
}
