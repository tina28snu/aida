using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class GoogleRequestModel
    {
        public List<ItemResult> itemListElement { get; set; }

        public class ItemResult
        {
            public ResultG result { get; set; }
        }

        public class ResultG
        {
            [JsonProperty("@id")]
            public string id { get; set; }
            public string name { get; set; }
            public Image image { get; set; }
            public string description { get; set; }
            public Description detailedDescription { get; set; }
        }

        public class Image
        {
            public string contentUrl { get; set; }
        }

        public class Description
        {
            public string articleBody { get; set; }
            public string url { get; set; }
        }
    }
}