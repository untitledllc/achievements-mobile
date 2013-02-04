using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using System.IO;
using System.Web;

namespace ItsBeta.WebControls
{
    public static class WebControls
    {
        public static HttpWebResponse PostMethod(string postedData, string postUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
            request.Method = "POST";
            request.Credentials = CredentialCache.DefaultCredentials;

            UTF8Encoding encoding = new UTF8Encoding();
            var bytes = encoding.GetBytes(postedData);

            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bytes.Length;

            using (var newStream = request.GetRequestStream())
            {
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();
            }
            return (HttpWebResponse)request.GetResponse();
        }

        public static string GetMethod(string getUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getUrl);
            request.ContentType = "application/json; charset=utf-8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream stream = response.GetResponseStream();

            var sr = new StreamReader(stream, Encoding.UTF8);

            var text = sr.ReadLine();
            var text2 = text.Replace(",", "\r\n");

            return text;//ParseJson(text2);
        }
    }
}