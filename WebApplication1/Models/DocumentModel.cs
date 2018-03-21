using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class DocumentModel
    {
        public string objectId { get; set; }
        public DocumentData documentData { get; set; }

        public class DocumentData
        {
            public string title { get; set; }
            public string url { get; set; }
            public string content { get; set; }
            public SE sementis_entities { get; set; }
            public List<ImagesA> images { get; set; }
        }

        public class ImagesA
        {
            public string thumbnail { get; set; }
            public string name { get; set; }
            public string url { get; set; }
        }

        public class SE
        {
            public List<Entity> entities { get; set; }
        }

        public class Entity
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