using System;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebApplication1.utils
{
    public class HttpAccessException : Exception
    {
        public HttpAccessException() : base() { }
        public HttpAccessException(string message) : base(message) { }
    }



    public class HttpAccess
    {
        public static class Json
        {
            public static T Get<T>(string url, NameValueCollection collection) where T : class
            {
                Uri uri = new Uri(url).AttachParameters(collection);
                return Get<T>(uri);
            }

            public static T Get<T>(Uri uri, NameValueCollection collection) where T : class
            {
                uri = uri.AttachParameters(collection);
                return Get<T>(uri);
            }

            public static T Get<T>(string url) where T : class
            {
                Uri uri = new Uri(url);
                return Get<T>(uri);
            }

            public static T Get<T>(Uri uri) where T : class
            {
                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(uri).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        HttpContent requestContent = response.Content;
                        string jsonContent = requestContent.ReadAsStringAsync().Result;

                        throw new HttpAccessException(jsonContent);
                    }

                    string json = response.Content.ReadAsStringAsync().Result;
                    T result = JsonConvert.DeserializeObject<T>(json);
                    return result;
                }
            }


            public static T Put<T>(string url, NameValueCollection collection, T element) where T : class
            {
                Uri uri = new Uri(url).AttachParameters(collection);
                return Put<T>(uri, element);
            }

            public static T Put<T>(Uri uri, NameValueCollection collection, T element) where T : class
            {
                uri = uri.AttachParameters(collection);
                return Put<T>(uri, element);
            }

            public static T Put<T>(string url, T element) where T : class
            {
                Uri uri = new Uri(url);
                return Put<T>(uri, element);
            }

            public static T Put<T>(Uri uri, T element)
            {
                using (var client = new HttpClient())
                {
                    using (var content = new StringContent(JsonConvert.SerializeObject(element, Formatting.None)))
                    {
                        var response = client.PutAsync(uri, content).Result;
                        if (!response.IsSuccessStatusCode)
                        {
                            HttpContent requestContent = response.Content;
                            string jsonContent = requestContent.ReadAsStringAsync().Result;

                            throw new HttpAccessException(jsonContent);
                        }

                        string json = response.Content.ReadAsStringAsync().Result;
                        T result = JsonConvert.DeserializeObject<T>(json);
                        return result;
                    }
                }
            }


            public static T Post<T>(string url, NameValueCollection collection, T element) where T : class
            {
                Uri uri = new Uri(url).AttachParameters(collection);
                return Post<T>(uri, element);
            }

            public static T Post<T>(Uri uri, NameValueCollection collection, T element) where T : class
            {
                uri = uri.AttachParameters(collection);
                return Post<T>(uri, element);
            }

            public static T Post<T>(string url, T element) where T : class
            {
                Uri uri = new Uri(url);
                return Post<T>(uri, element);
            }

            public static T Post<T>(Uri uri, T element)
            {
                using (var client = new HttpClient())
                {
                    using (var content = new StringContent(JsonConvert.SerializeObject(element, Formatting.None)))
                    {
                        var response = client.PostAsync(uri, content).Result;
                        if (!response.IsSuccessStatusCode)
                        {
                            HttpContent requestContent = response.Content;
                            string jsonContent = requestContent.ReadAsStringAsync().Result;

                            throw new HttpAccessException(jsonContent);
                        }

                        string json = response.Content.ReadAsStringAsync().Result;
                        T result = JsonConvert.DeserializeObject<T>(json);
                        return result;
                    }
                }
            }


            public static T Delete<T>(string url, NameValueCollection collection) where T : class
            {
                Uri uri = new Uri(url).AttachParameters(collection);
                return Delete<T>(uri);
            }

            public static T Delete<T>(Uri uri, NameValueCollection collection) where T : class
            {
                uri = uri.AttachParameters(collection);
                return Delete<T>(uri);
            }

            public static T Delete<T>(string url) where T : class
            {
                Uri uri = new Uri(url);
                return Delete<T>(uri);
            }

            public static T Delete<T>(Uri uri)
            {
                using (var client = new HttpClient())
                {
                    var response = client.DeleteAsync(uri).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        HttpContent requestContent = response.Content;
                        string jsonContent = requestContent.ReadAsStringAsync().Result;

                        throw new HttpAccessException(jsonContent);
                    }

                    string json = response.Content.ReadAsStringAsync().Result;
                    T result = JsonConvert.DeserializeObject<T>(json);
                    return result;
                }
            }
        }



        public class File
        {
            public string fileName { get; set; }
            public byte[] byteArray { get; set; }

            public static File Get(string url, NameValueCollection collection)
            {
                Uri uri = new Uri(url).AttachParameters(collection);
                return Get(uri);
            }

            public static File Get(Uri uri, NameValueCollection collection)
            {
                uri = uri.AttachParameters(collection);
                return Get(uri);
            }

            public static File Get(string url)
            {
                Uri uri = new Uri(url);
                return Get(uri);
            }

            public static File Get(Uri uri)
            {
                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(uri).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        HttpContent requestContent = response.Content;
                        string jsonContent = requestContent.ReadAsStringAsync().Result;

                        throw new HttpAccessException(jsonContent);
                    }

                    string filename = response.Content.Headers.ContentDisposition.FileName;
                    byte[] byteArray = response.Content.ReadAsByteArrayAsync().Result;

                    return new File { fileName = filename, byteArray = byteArray };
                }
            }


            public static string Put(string url, NameValueCollection collection, byte[] file)
            {
                Uri uri = new Uri(url).AttachParameters(collection);
                return Put(uri, file);
            }

            public static string Put(Uri uri, NameValueCollection collection, byte[] file)
            {
                uri = uri.AttachParameters(collection);
                return Put(uri, file);
            }

            public static string Put(string url, byte[] file)
            {
                Uri uri = new Uri(url);
                return Put(uri, file);
            }

            public static string Put(Uri uri, byte[] file)
            {
                using (var client = new HttpClient())
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        content.Add(new StreamContent(new MemoryStream(file)));
                        var response = client.PutAsync(uri, content).Result;

                        if (!response.IsSuccessStatusCode)
                        {
                            HttpContent requestContent = response.Content;
                            string jsonContent = requestContent.ReadAsStringAsync().Result;

                            throw new HttpAccessException(jsonContent);
                        }

                        return response.Content.ReadAsStringAsync().Result;
                    }
                }
            }


            public static string Post(string url, NameValueCollection collection, byte[] file)
            {
                Uri uri = new Uri(url).AttachParameters(collection);
                return Post(uri, file);
            }

            public static string Post(Uri uri, NameValueCollection collection, byte[] file)
            {
                uri = uri.AttachParameters(collection);
                return Post(uri, file);
            }

            public static string Post(string url, byte[] file)
            {
                Uri uri = new Uri(url);
                return Post(uri, file);
            }

            public static string Post(Uri uri, byte[] file)
            {
                using (var client = new HttpClient())
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        content.Add(new StreamContent(new MemoryStream(file)));
                        var response = client.PostAsync(uri, content).Result;

                        if (!response.IsSuccessStatusCode)
                        {
                            HttpContent requestContent = response.Content;
                            string jsonContent = requestContent.ReadAsStringAsync().Result;

                            throw new HttpAccessException(jsonContent);
                        }
                        return response.Content.ReadAsStringAsync().Result;
                    }
                }
            }


            public static string Delete(string url, NameValueCollection collection)
            {
                Uri uri = new Uri(url).AttachParameters(collection);
                return Delete(uri);
            }

            public static string Delete(Uri uri, NameValueCollection collection)
            {
                uri = uri.AttachParameters(collection);
                return Delete(uri);
            }

            public static string Delete(string url)
            {
                Uri uri = new Uri(url);
                return Delete(uri);
            }

            public static string Delete(Uri uri)
            {
                using (var client = new HttpClient())
                {
                    var response = client.DeleteAsync(uri).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        HttpContent requestContent = response.Content;
                        string jsonContent = requestContent.ReadAsStringAsync().Result;

                        throw new HttpAccessException(jsonContent);
                    }
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }



        public static class Authentification
        {
            public static class Json
            {
                public static T Get<T>(string url, NameValueCollection collection, string login, string password) where T : class
                {
                    Uri uri = new Uri(url).AttachParameters(collection);
                    return Get<T>(uri, login, password);
                }

                public static T Get<T>(Uri uri, NameValueCollection collection, string login, string password) where T : class
                {
                    uri = uri.AttachParameters(collection);
                    return Get<T>(uri, login, password);
                }

                public static T Get<T>(string url, string login, string password) where T : class
                {
                    Uri uri = new Uri(url);
                    return Get<T>(uri, login, password);
                }

                public static T Get<T>(Uri uri, string login, string password) where T : class
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                            "Basic",
                            Convert.ToBase64String(
                                System.Text.ASCIIEncoding.ASCII.GetBytes(
                                    string.Format("{0}:{1}", login, password))));

                        var response = client.GetAsync(uri).Result;
                        if (!response.IsSuccessStatusCode)
                        {
                            HttpContent requestContent = response.Content;
                            string jsonContent = requestContent.ReadAsStringAsync().Result;

                            throw new HttpAccessException(jsonContent);
                        }

                        string json = response.Content.ReadAsStringAsync().Result;
                        T result = JsonConvert.DeserializeObject<T>(json);
                        return result;
                    }
                }


                public static T Put<T>(string url, NameValueCollection collection, T element, string login, string password) where T : class
                {
                    Uri uri = new Uri(url).AttachParameters(collection);
                    return Put<T>(uri, element, login, password);
                }

                public static T Put<T>(Uri uri, NameValueCollection collection, T element, string login, string password) where T : class
                {
                    uri = uri.AttachParameters(collection);
                    return Put<T>(uri, element, login, password);
                }

                public static T Put<T>(string url, T element, string login, string password) where T : class
                {
                    Uri uri = new Uri(url);
                    return Put<T>(uri, element, login, password);
                }

                public static T Put<T>(Uri uri, T element, string login, string password)
                {
                    using (var client = new HttpClient())
                    {
                        using (var content = new StringContent(JsonConvert.SerializeObject(element, Formatting.None)))
                        {
                            client.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue(
                                "Basic",
                                Convert.ToBase64String(
                                    System.Text.ASCIIEncoding.ASCII.GetBytes(
                                        string.Format("{0}:{1}", login, password))));

                            var response = client.PutAsync(uri, content).Result;
                            if (!response.IsSuccessStatusCode)
                            {
                                HttpContent requestContent = response.Content;
                                string jsonContent = requestContent.ReadAsStringAsync().Result;

                                throw new HttpAccessException(jsonContent);
                            }

                            string json = response.Content.ReadAsStringAsync().Result;
                            T result = JsonConvert.DeserializeObject<T>(json);
                            return result;
                        }
                    }
                }


                public static T Post<T>(string url, NameValueCollection collection, T element, string login, string password) where T : class
                {
                    Uri uri = new Uri(url).AttachParameters(collection);
                    return Post<T>(uri, element, login, password);
                }

                public static T Post<T>(Uri uri, NameValueCollection collection, T element, string login, string password) where T : class
                {
                    uri = uri.AttachParameters(collection);
                    return Post<T>(uri, element, login, password);
                }

                public static T Post<T>(string url, T element, string login, string password) where T : class
                {
                    Uri uri = new Uri(url);
                    return Post<T>(uri, element, login, password);
                }

                public static T Post<T>(Uri uri, T element, string login, string password)
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                            "Basic",
                            Convert.ToBase64String(
                                System.Text.ASCIIEncoding.ASCII.GetBytes(
                                    string.Format("{0}:{1}", login, password))));

                        using (var content = new StringContent(JsonConvert.SerializeObject(element, Formatting.None)))
                        {
                            var response = client.PostAsync(uri, content).Result;
                            if (!response.IsSuccessStatusCode)
                            {
                                HttpContent requestContent = response.Content;
                                string jsonContent = requestContent.ReadAsStringAsync().Result;

                                throw new HttpAccessException(jsonContent);
                            }

                            string json = response.Content.ReadAsStringAsync().Result;
                            T result = JsonConvert.DeserializeObject<T>(json);
                            return result;
                        }
                    }
                }


                public static T Delete<T>(string url, NameValueCollection collection, string login, string password) where T : class
                {
                    Uri uri = new Uri(url).AttachParameters(collection);
                    return Delete<T>(uri, login, password);
                }

                public static T Delete<T>(Uri uri, NameValueCollection collection, string login, string password) where T : class
                {
                    uri = uri.AttachParameters(collection);
                    return Delete<T>(uri, login, password);
                }

                public static T Delete<T>(string url, string login, string password) where T : class
                {
                    Uri uri = new Uri(url);
                    return Delete<T>(uri, login, password);
                }

                public static T Delete<T>(Uri uri, string login, string password)
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                            "Basic",
                            Convert.ToBase64String(
                                System.Text.ASCIIEncoding.ASCII.GetBytes(
                                    string.Format("{0}:{1}", login, password))));

                        var response = client.DeleteAsync(uri).Result;
                        if (!response.IsSuccessStatusCode)
                        {
                            HttpContent requestContent = response.Content;
                            string jsonContent = requestContent.ReadAsStringAsync().Result;

                            throw new HttpAccessException(jsonContent);
                        }

                        string json = response.Content.ReadAsStringAsync().Result;
                        T result = JsonConvert.DeserializeObject<T>(json);
                        return result;
                    }
                }
            }

            public class File
            {
                public string fileName { get; set; }
                public byte[] byteArray { get; set; }

                public static File Get(string url, NameValueCollection collection, string login, string password)
                {
                    Uri uri = new Uri(url).AttachParameters(collection);
                    return Get(uri, login, password);
                }

                public static File Get(Uri uri, NameValueCollection collection, string login, string password)
                {
                    uri = uri.AttachParameters(collection);
                    return Get(uri, login, password);
                }

                public static File Get(string url, string login, string password)
                {
                    Uri uri = new Uri(url);
                    return Get(uri, login, password);
                }

                public static File Get(Uri uri, string login, string password)
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                            "Basic",
                            Convert.ToBase64String(
                                System.Text.ASCIIEncoding.ASCII.GetBytes(
                                    string.Format("{0}:{1}", login, password))));

                        var response = client.GetAsync(uri).Result;
                        if (!response.IsSuccessStatusCode)
                        {
                            HttpContent requestContent = response.Content;
                            string jsonContent = requestContent.ReadAsStringAsync().Result;

                            throw new HttpAccessException(jsonContent);
                        }

                        string filename = response.Content.Headers.ContentDisposition.FileName;
                        byte[] byteArray = response.Content.ReadAsByteArrayAsync().Result;

                        return new File { fileName = filename, byteArray = byteArray };
                    }
                }


                public static string Put(string url, NameValueCollection collection, byte[] file, string fileName, string login, string password)
                {
                    Uri uri = new Uri(url).AttachParameters(collection);
                    return Put(uri, file, fileName, login, password);
                }

                public static string Put(Uri uri, NameValueCollection collection, byte[] file, string fileName, string login, string password)
                {
                    uri = uri.AttachParameters(collection);
                    return Put(uri, file, fileName, login, password);
                }

                public static string Put(string url, byte[] file, string fileName, string login, string password)
                {
                    Uri uri = new Uri(url);
                    return Put(uri, file, fileName, login, password);
                }

                public static string Put(Uri uri, byte[] file, string fileName, string login, string password)
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                            "Basic",
                            Convert.ToBase64String(
                                System.Text.ASCIIEncoding.ASCII.GetBytes(
                                    string.Format("{0}:{1}", login, password))));

                        using (var content = new MultipartFormDataContent())
                        {
                            content.Add(new StreamContent(new MemoryStream(file)), fileName, fileName);
                            var response = client.PutAsync(uri, content).Result;

                            if (!response.IsSuccessStatusCode)
                            {
                                HttpContent requestContent = response.Content;
                                string jsonContent = requestContent.ReadAsStringAsync().Result;

                                throw new HttpAccessException(jsonContent);
                            }

                            return response.Content.ReadAsStringAsync().Result;
                        }
                    }
                }


                public static string Post(string url, NameValueCollection collection, byte[] file, string fileName, string login, string password)
                {
                    Uri uri = new Uri(url).AttachParameters(collection);
                    return Post(uri, file, fileName, login, password);
                }

                public static string Post(Uri uri, NameValueCollection collection, byte[] file, string fileName, string login, string password)
                {
                    uri = uri.AttachParameters(collection);
                    return Post(uri, file, fileName, login, password);
                }

                public static string Post(string url, byte[] file, string fileName, string login, string password)
                {
                    Uri uri = new Uri(url);
                    return Post(uri, file, fileName, login, password);
                }

                public static string Post(Uri uri, byte[] file, string fileName, string login, string password)
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                            "Basic",
                            Convert.ToBase64String(
                                System.Text.ASCIIEncoding.ASCII.GetBytes(
                                    string.Format("{0}:{1}", login, password))));

                        using (var content = new MultipartFormDataContent())
                        {
                            content.Add(new StreamContent(new MemoryStream(file)), fileName, fileName);
                            var response = client.PostAsync(uri, content).Result;

                            if (!response.IsSuccessStatusCode)
                            {
                                HttpContent requestContent = response.Content;
                                string jsonContent = requestContent.ReadAsStringAsync().Result;

                                throw new HttpAccessException(jsonContent);
                            }
                            return response.Content.ReadAsStringAsync().Result;
                        }
                    }
                }


                public static string Delete(string url, NameValueCollection collection, string login, string password)
                {
                    Uri uri = new Uri(url).AttachParameters(collection);
                    return Delete(uri, login, password);
                }

                public static string Delete(Uri uri, NameValueCollection collection, string login, string password)
                {
                    uri = uri.AttachParameters(collection);
                    return Delete(uri, login, password);
                }

                public static string Delete(string url, string login, string password)
                {
                    Uri uri = new Uri(url);
                    return Delete(uri, login, password);
                }

                public static string Delete(Uri uri, string login, string password)
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                            "Basic",
                            Convert.ToBase64String(
                                System.Text.ASCIIEncoding.ASCII.GetBytes(
                                    string.Format("{0}:{1}", login, password))));

                        var response = client.DeleteAsync(uri).Result;
                        if (!response.IsSuccessStatusCode)
                        {
                            HttpContent requestContent = response.Content;
                            string jsonContent = requestContent.ReadAsStringAsync().Result;

                            throw new HttpAccessException(jsonContent);
                        }
                        return response.Content.ReadAsStringAsync().Result;
                    }
                }
            }
        }



        public static string Get(string url, NameValueCollection collection)
        {
            Uri uri = new Uri(url).AttachParameters(collection);
            return Get(uri);
        }

        public static string Get(Uri uri, NameValueCollection collection)
        {
            uri = uri.AttachParameters(collection);
            return Get(uri);
        }

        public static string Get(string url)
        {
            Uri uri = new Uri(url);
            return Get(uri);
        }

        public static string Get(Uri uri)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(uri).Result;
                if (!response.IsSuccessStatusCode)
                {
                    HttpContent requestContent = response.Content;
                    string jsonContent = requestContent.ReadAsStringAsync().Result;

                    throw new HttpAccessException(jsonContent);
                }

                return response.Content.ReadAsStringAsync().Result;
            }
        }



        public static string Put(string url, NameValueCollection collection, string element)
        {
            Uri uri = new Uri(url).AttachParameters(collection);
            return Put(uri, element);
        }

        public static string Put(Uri uri, NameValueCollection collection, string element)
        {
            uri = uri.AttachParameters(collection);
            return Put(uri, element);
        }

        public static string Put(string url, string element)
        {
            Uri uri = new Uri(url);
            return Put(uri, element);
        }

        public static string Put(Uri uri, string element)
        {
            using (var client = new HttpClient())
            {
                using (var content = new StringContent(element))
                {
                    var response = client.PutAsync(uri, content).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        HttpContent requestContent = response.Content;
                        string jsonContent = requestContent.ReadAsStringAsync().Result;

                        throw new HttpAccessException(jsonContent);
                    }

                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }



        public static string Post(string url, NameValueCollection collection, string element)
        {
            Uri uri = new Uri(url).AttachParameters(collection);
            return Post(uri, element);
        }

        public static string Post(Uri uri, NameValueCollection collection, string element)
        {
            uri = uri.AttachParameters(collection);
            return Post(uri, element);
        }

        public static string Post(string url, string element)
        {
            Uri uri = new Uri(url);
            return Post(uri, element);
        }

        public static string Post(Uri uri, string element)
        {
            using (var client = new HttpClient())
            {
                using (var content = new StringContent(element))
                {
                    var response = client.PostAsync(uri, content).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        HttpContent requestContent = response.Content;
                        string jsonContent = requestContent.ReadAsStringAsync().Result;

                        throw new HttpAccessException(jsonContent);
                    }

                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }
        


        public static string Delete(string url, NameValueCollection collection)
        {
            Uri uri = new Uri(url).AttachParameters(collection);
            return Delete(uri);
        }

        public static string Delete(Uri uri, NameValueCollection collection)
        {
            uri = uri.AttachParameters(collection);
            return Delete(uri);
        }

        public static string Delete(string url)
        {
            Uri uri = new Uri(url);
            return Delete(uri);
        }

        public static string Delete(Uri uri)
        {
            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync(uri).Result;
                if (!response.IsSuccessStatusCode)
                {
                    HttpContent requestContent = response.Content;
                    string jsonContent = requestContent.ReadAsStringAsync().Result;

                    throw new HttpAccessException(jsonContent);
                }

                return response.Content.ReadAsStringAsync().Result;
            }
        }

    }
}


