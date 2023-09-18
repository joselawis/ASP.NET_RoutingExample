using System.Text.RegularExpressions;

namespace RoutingExample.CustomConstraints
{
    public class MonthsCustomConstraint : IRouteConstraint
    {
        public MonthsCustomConstraint()
        {
        }

        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            // Verify if the value exists
            if (!values.ContainsKey(routeKey))
            {
                return false;
            }
            Regex regex = new Regex($"^(apr|jul|oct|jan)$");
            string? monthValue = Convert.ToString(values[routeKey]);

            return monthValue != null && regex.IsMatch(monthValue);
        }
    }
}

