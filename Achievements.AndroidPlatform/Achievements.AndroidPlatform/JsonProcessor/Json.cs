using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Achievements.AndroidPlatform.JsonProcessor
{
    public class JSonProcessor
    {
        public class JSonCategories
        {
            string _jsonResponse;

            public JToken jToken;

            public JSonCategories(string access_token)
            {
                _jsonResponse = WebControls.WebControls.GetMethod(
                    String.Format("http://www.itsbeta.com/info/categories.json?access_token={0}", 
                    access_token));

                jToken = JToken.Parse(_jsonResponse);
            }

            public int Count
            {
                get
                {
                    return jToken.Count();
                }
                private set {}
            } 
        }

        public class JSonProjects
        {
            string _jsonResponse;

            public JToken jToken;

            public JSonProjects(string access_token, string categoryId)
            {
                _jsonResponse = WebControls.WebControls.GetMethod(
                    String.Format("http://www.itsbeta.com/info/projects.json?access_token={0}&category_id={1}",
                    access_token, categoryId));

                jToken = JToken.Parse(_jsonResponse);
            }

            public int Count
            {
                get
                {
                    return jToken.Count();
                }
                private set { }
            }
        }

    }
}
