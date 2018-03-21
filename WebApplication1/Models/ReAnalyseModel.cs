using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class ReAnalyseModel
    {
        public string id { get; set; }
        public res result { get; set;}

        public class res
        {
            public List<entity> entities { get; set; }
        }

        public class entity
        {
            public List<Occurrences> occurrences { get; set; }
            public string commonName { get; set; }
            public List<Images> images { get; set; }
            public string freebaseId { get; set; }
            public string description { get; set; }
        }

        public class Occurrences
        {
            public string value { get; set; }
            public int start { get; set; }
            public int end { get; set; }
        }

        public class Images
        {
            public string title { get; set; }
            public string url { get; set; }
        }
    }
}