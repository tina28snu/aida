using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Repository;
using WebApplication1.utils;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserModel UM)
        {
            //UM.idUser = UM.ConnectUser();
            //if (UM.idUser != -1)
            //{
                Utils.ConnectedUSer = UM;
                return RedirectToAction("Entities", "Home");
            //}
            //else
            //{
            //    ViewBag.ErrMsg = "Login or Password incorrect";
            //    return RedirectToAction("Index", "Home");
            //}
        }


        public ActionResult Entities()
        {
            string corpusId = "5832e38c25218103c36f384a";
            string query = "{\"limit\":20, \"sort\":[{\"_id\": -1}]}";
            string URI = "http://51.255.95.132:6880/api/search";
            string myParameters = "key=broccoli&corpusId=" + corpusId + "&query=" + query;

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string HtmlResult = wc.UploadString(URI, myParameters);

                byte[] bytes = Encoding.Default.GetBytes(HtmlResult);
                HtmlResult = Encoding.UTF8.GetString(bytes);

                ViewBag.corpusId = corpusId;
                LatestDocumentsModel res = JsonConvert.DeserializeObject<LatestDocumentsModel>(HtmlResult);
                return View(res);
            }
        }


        public ActionResult Article(string corpusId, string documentId, string check, int index = -1)
        {
            DocumentModel doc = new DocumentModel();
            AidaRepository ar = new AidaRepository();
            ViewBag.index = index;
            if (check == "0")
            {
                corpusId = "5832e38c25218103c36f384a";
                doc = ar.GetDocument(corpusId, documentId);
            }
            else
            {
                if (corpusId == null)
                {
                    return View();
                }
                doc = ar.GetDocument(corpusId, documentId);
            }
            return View(doc);
        }
        

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}