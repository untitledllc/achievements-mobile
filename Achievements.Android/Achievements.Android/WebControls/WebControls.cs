using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net;

using Newtonsoft.Json;
using System.IO;

namespace Achievements.Android.WebControls
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
        //var response = PostMethod(postedData, ApiUrl);
        //if (response != null)
        //{
        //  var strreader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
        //  var responseToString = strreader.ReadToEnd();
        //}
    }
}