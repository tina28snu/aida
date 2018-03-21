using System.Collections.Specialized;
using WebApplication1.Models;
using WebApplication1.utils;

namespace WebApplication1.Repository
{
    public class AidaRepository
    {
        public DocumentModel GetDocument(string corpusId, string documentId)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("key", "broccoli");
            nvc.Add("corpusId", corpusId);
            nvc.Add("documentId", documentId);

            return HttpAccess.Json.Get<DocumentModel>("http://51.255.95.132:6880/api/corpus/document", nvc);
        }

        

    }
}