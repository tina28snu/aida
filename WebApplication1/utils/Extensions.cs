using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;

namespace WebApplication1.utils
{
    public static class Extensions
    {
        public static Uri AttachParameters(this Uri uri, NameValueCollection parameters)
        {
            if (parameters.Count == 0)
                return uri;

            var query = HttpUtility.ParseQueryString(string.Empty);

            for (int index = 0; index < parameters.Count; ++index)
            {
                query[parameters.AllKeys[index]] = parameters[index];
            }
            string queryString = query.ToString();

            return new Uri(uri + "?" + queryString);
        }

        public static Dictionary<string, string> Parameters(this Uri self)
        {
            return String.IsNullOrEmpty(self.Query)
              ? new Dictionary<string, string>()
              : self.Query.Substring(1).Split('&').ToDictionary(
                  p => p.Split('=')[0],
                  p => p.Split('=')[1]
              );
        }
    }
}