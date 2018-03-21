using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    public class UpdateController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Suggestions(string commonName)
        {
            GoogleRequestModel gr = new GoogleRequestModel();
            GoogleRepository grep = new GoogleRepository();
            gr = grep.GetRequests(commonName);
            ViewBag.commonName = commonName;
            return PartialView("~/Views/Shared/_RequestsGoogle.cshtml", gr);
        }

        [HttpPost]
        public ActionResult Article(string textToAnalyse)
        {
            ViewBag.context = textToAnalyse;
            textToAnalyse = textToAnalyse.Replace("\"", " ");
            String input = "[{\"id\":\"1\",\"text\":\"" + @textToAnalyse + "\"}]";
            string URI = "http://192.168.11.76:8080/api/model/infer/entities/freebase";
            string myParameters = "key=broccoli&input=" + input;
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string HtmlResult = wc.UploadString(URI, myParameters);

                //byte[] bytes = Encoding.Default.GetBytes(HtmlResult);
                //HtmlResult = Encoding.UTF8.GetString(bytes);

                List<ReAnalyseModel> res = JsonConvert.DeserializeObject<List<ReAnalyseModel>>(HtmlResult);
                ReAnalyseModel tmp = res[0];
                return PartialView("~/Views/Shared/_ReAnalyse.cshtml", tmp);
            }
        }

        [HttpPost]
        public ActionResult Update (DocumentModel article)
        {
            // Delete code below and update in AIDA

            var success = false;

            if (success)
            {
                ViewBag.msg = "success";
            }
            else
            {
                ViewBag.msg = "error";
            }

            return PartialView("~/Views/Shared/_Update.cshtml");
        }

        [HttpPost]
        public ActionResult NextArticle (int index = -1)
        {
            if (index < 39)
            {
                ViewBag.index = index + 1;
                string corpusId = "5832e38c25218103c36f384a";
                string query = "{\"limit\":40, \"sort\":[{\"_id\": -1}]}";
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
                    DocumentModel doc = res.documents[index + 1];
                    return View("~/Views/Home/Article.cshtml", doc);
                }
            }
            else
            {
                return RedirectToAction("Entities", "Home");
            }
            
        }
    }
}