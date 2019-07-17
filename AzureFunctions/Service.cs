using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Configuration;
using System.IO;
using Newtonsoft.Json.Linq;

namespace AzureFunctions
{
    class Service
    {
        public static string ViewEmployees()
        {
            Uri getUrl = new Uri($"{ConfigurationManager.AppSettings["BaseAddress"]}" +
                                 $"{ConfigurationManager.AppSettings["ViewEmployees"]}");
            return InvokeGet(getUrl);
        }

        public static string ViewEmployeeByID(string id)
        {
            Uri getUrl = new Uri($"{ConfigurationManager.AppSettings["BaseAddress"]}" +
                                 $"{string.Format(ConfigurationManager.AppSettings["ViewEmployee"],id)}");
            return InvokeGet(getUrl);
        }

        public static string CreateEmployee(Object postObject)
        {
            Uri getUrl = new Uri($"{ConfigurationManager.AppSettings["BaseAddress"]}" +
                                 $"{ConfigurationManager.AppSettings["CreateEmployee"]}");
            return InvokePost(getUrl,postObject);
        }

        public static string UpdateEmployee(Object postObject, string id)
        {
            Uri getUrl = new Uri($"{ConfigurationManager.AppSettings["BaseAddress"]}" +
                                 $"{string.Format(ConfigurationManager.AppSettings["Update"],id)}");
            return InvokePut(getUrl, postObject);
        }

        public static string DeleteEmployee(string id)
        {
            if (ViewEmployeeByID(id) == "false")
            {
                return "User not found";
            }
            Uri getUrl = new Uri($"{ConfigurationManager.AppSettings["BaseAddress"]}" +
                                 $"{string.Format(ConfigurationManager.AppSettings["Delete"], id)}");
            return InvokeDelete(getUrl);
        }

        private static string InvokeGet(Uri url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Timeout = 1000 * 60;
            request.ContentType = "application/json";

            using ( var response = (HttpWebResponse)request.GetResponse())
            {
                using( var reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static string InvokePost(Uri url, object postObject)
        {
            var postData = Encoding.UTF8.GetBytes(JObject.FromObject(postObject).ToString());

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Timeout = 1000 * 60;
            request.ContentType = "application/json";

            using (var stream = request.GetRequestStream())
            {
                stream.Write(postData, 0, postData.Length);
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var stream = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        var responseContent = stream.ReadToEnd();

                        return responseContent;
                    }
                }
            }
        }

        private static string InvokePut(Uri url, object postObject)
        {
            var postData = Encoding.UTF8.GetBytes(JObject.FromObject(postObject).ToString());

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";
            request.Timeout = 1000 * 60;
            request.ContentType = "application/json";

            using (var stream = request.GetRequestStream())
            {
                stream.Write(postData, 0, postData.Length);
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var stream = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        return stream.ReadToEnd();
                    }
                }
            }
        }

        private static string InvokeDelete(Uri url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "DELETE";
            request.Timeout = 1000 * 60;
            request.ContentType = "application/json";

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
