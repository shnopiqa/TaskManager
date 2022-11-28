using DryIoc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Controls;
using TaskManagerCourse.Client.Models;

namespace TaskManagerCourse.Client.Services
{
    public abstract class CommonRequestService
    {
        public const string HOST = "http://localhost:65177/api/";

        public string _usersContollerUrl = HOST + "users";
       protected string GetDataByUrl(HttpMethod method, string url, AuthToken token, string userName = null, string paswword = null, 
           Dictionary<string, string> parametrs = null)
        {
            //string result = string.Empty;
            //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            //request.Method = method.Method;
            //if (userName != null && paswword != null)
            //{
            //    string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(userName + ":" + paswword));
            //    request.Headers.Add("Authorization", "Basic " + encoded);
            //}
            //else if (token != null)
            //{
            //    request.Headers.Add("Authorization", "Bearer " + token.access_token);
            //}
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            //{
            //    string responsestr = reader.ReadToEnd();

            //    result = responsestr;
            //}
            //return result;

            WebClient client = new WebClient();
            if (userName != null && paswword != null)
            {
                string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(userName + ":" + paswword));
                client.Headers.Add("Authorization", "Basic " + encoded);
            }
            else if (token != null)
            {
                client.Headers.Add("Authorization", "Bearer " + token.access_token);
            }

            if (parametrs != null)
            {
                foreach (var key in parametrs.Keys)
                    client.QueryString.Add(key, parametrs[key]);
            }

            byte[] data = Array.Empty<byte>();
            try
            {
                if (method == HttpMethod.Post)
                    data = client.UploadValues(url, method.Method, client.QueryString);

                if (method == HttpMethod.Get)
                    data = client.DownloadData(url);
            }

            catch { }
            string result1 = UnicodeEncoding.UTF8.GetString(data);
            return result1;

        }
        protected HttpStatusCode SendhDataByUrl(HttpMethod method, string url, AuthToken token, string data = null)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.access_token);

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            if (method == HttpMethod.Post)
                result = client.PostAsync(url, content).Result;

            if (method == HttpMethod.Patch)
                result = client.PatchAsync(url, content).Result;
            return result.StatusCode;
        }
        protected HttpStatusCode DeleteDataByUrl(string url, AuthToken token)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.access_token);
            result = client.DeleteAsync(url).Result;

            return result.StatusCode;
        }

    }
}
