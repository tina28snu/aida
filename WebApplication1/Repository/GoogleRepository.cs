using System.Collections.Specialized;
using WebApplication1.Models;
using WebApplication1.utils;

namespace WebApplication1.Repository
{
    public class GoogleRepository
    {
        public GoogleRequestModel GetRequests(string query)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("query", query);
            nvc.Add("key", "AIzaSyDtfZ2BY35AfmCy6DGtV4fRc0RjgmN6NPk");
            nvc.Add("limit", "10");
            nvc.Add("indent", "True");

            return HttpAccess.Json.Get<GoogleRequestModel>("https://kgsearch.googleapis.com/v1/entities:search", nvc);
        }
    }
}