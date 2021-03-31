using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DocSpiderTest.Helpers {
    public static class UrlHelperExtensions {
        /// <summary>
        /// Get current URL with substituted values while preserving query string
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="substitutes">Query string parameters or route data paremers. E.g. new { action="Index", sort = "asc"}</param>
        /// <returns></returns>
        public static string Current(this IUrlHelper helper, object substitutes) {
            var routeData   = new RouteValueDictionary(helper.ActionContext.RouteData.Values);
            var queryString = helper.ActionContext.HttpContext.Request.Query;

            //add query string parameters to the route data
            foreach (var (key, value) in queryString) {
                if (!string.IsNullOrEmpty(queryString[key])) {
                    //rd[param.Key] = qs[param.Value]; // does not assign the values!
                    routeData.Add(key, value);
                }
            }

            // override parameters we're changing in the route data
            //The unmatched parameters will be added as query string.
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(substitutes.GetType())) {
                var value = property.GetValue(substitutes);
                if (string.IsNullOrEmpty(value?.ToString())) {
                    routeData.Remove(property.Name);
                }
                else {
                    routeData[property.Name] = value;
                }
            }

            string url = helper.RouteUrl(routeData);
            return url;
        }
    }
}