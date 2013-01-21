using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ItsBeta.Json
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
                    String.Format("http://www.itsbeta.com/s/info/categories.json?access_token={0}", 
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

            public JSonProjects(string access_token)
            {
                _jsonResponse = WebControls.WebControls.GetMethod(
                    String.Format("http://www.itsbeta.com/s/info/projects.json?access_token={0}",
                    access_token));

                jToken = JToken.Parse(_jsonResponse);
            }

            public JSonProjects(string access_token, string category_name)
            {
                _jsonResponse = WebControls.WebControls.GetMethod(
                    String.Format("http://www.itsbeta.com/s/info/projects.json?access_token={0}&category_name={1}",
                    access_token, category_name));

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

        public class JSonObjTypes
        {
            string _jsonResponse;

            public JToken jToken;

            public JSonObjTypes(string access_token)
            {
                _jsonResponse = WebControls.WebControls.GetMethod(
                    String.Format("http://www.itsbeta.com/s/info/objtypes.json?access_token={0}",
                    access_token));

                jToken = JToken.Parse(_jsonResponse);
            }


            public JSonObjTypes(string access_token, string unit_id, bool isParentId)
            {
                if (isParentId)
                {
                    _jsonResponse = WebControls.WebControls.GetMethod(
                    String.Format("http://www.itsbeta.com/s/info/objtypes.json?access_token={0}&parent_id={1}",
                    access_token, unit_id));
                }
                else
                {
                    _jsonResponse = WebControls.WebControls.GetMethod(
                        String.Format("http://www.itsbeta.com/s/info/objtypes.json?access_token={0}&project_id={1}",
                        access_token, unit_id));
                }

                jToken = JToken.Parse(_jsonResponse);
            }
            

            public JSonObjTypes(string access_token, string project_id, string parent_id)
            {
                _jsonResponse = WebControls.WebControls.GetMethod(
                    String.Format("http://www.itsbeta.com/s/info/objtypes.json?access_token={0}&project_id={1}&parent_id={2}",
                    access_token, project_id, parent_id));

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

        public class JSonObjTemplates
        {
            string _jsonResponse;

            public JToken jToken;

            public JSonObjTemplates(string access_token, string objtype_id)
            {
                _jsonResponse = WebControls.WebControls.GetMethod(
                    String.Format("http://www.itsbeta.com/s/info/objtemplates.json?access_token={0}&objtype_id={1}",
                    access_token, objtype_id));

                jToken = JToken.Parse(_jsonResponse);
            }

            public JSonObjTemplates(string access_token, string objtype_id, string project_id)
            {
                _jsonResponse = WebControls.WebControls.GetMethod(
                    String.Format("http://www.itsbeta.com/s/info/objtemplates.json?access_token={0}&objtype_id={1}&project_id={2}",
                    access_token, objtype_id, project_id));

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

        public class JSonObj
        {
            string _jsonResponse;

            public JToken jToken;

            public JSonObj(string access_token, string objtemplate_id, string player_id)
            {
                _jsonResponse = WebControls.WebControls.GetMethod(
                    String.Format("http://www.itsbeta.com/s/info/objs.json?access_token={0}&objtemplate_id={1}&player_id={2}",
                    access_token, objtemplate_id, player_id));

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

        public class JSonPlayerId
        {
            string _jsonResponse;

            public JToken jToken;

            public JSonPlayerId(string access_token, string type, string id)
            {
                _jsonResponse = WebControls.WebControls.GetMethod(
                    String.Format("http://www.itsbeta.com/s/info/playerid.json?access_token={0}&type={1}&id={2}",
                    access_token, type, id));

                jToken = JToken.Parse(_jsonResponse);
            }
        }

        public class JSonPostAchive
        {
            string _jsonResponse;

            public JToken jToken;

            public JSonPostAchive(string access_token, string category, string project, string badge_name)
            {
                _jsonResponse = WebControls.WebControls.GetMethod(
                    String.Format("http://www.itsbeta.com/s/{1}/{2}/achieves/postachieve.json?access_token={0}&badge_name={3}",
                    access_token, category, project, badge_name));

                jToken = JToken.Parse(_jsonResponse);
            }
        }

        public class JSonPostToFb
        {
            string _jsonResponse;

            public JToken jToken;

            public JSonPostToFb(string access_token, string category, string project, string user_id, string user_token, string activation_code)
            {
                _jsonResponse = WebControls.WebControls.GetMethod(
                    String.Format("http://www.itsbeta.com/s/{1}/{2}/achieves/posttofb.json?access_token={0}&user_id={3}&user_token={4}&activation_code={5}",
                    access_token, category, project, user_id, user_token, activation_code));

                jToken = JToken.Parse(_jsonResponse);
            }
        }

        public class JSonPostToFbOnce
        {
            string _jsonResponse;

            public JToken jToken;

            public JSonPostToFbOnce(string access_token, string category, string project, 
                string user_id, string user_token, string activation_code)
            {
                _jsonResponse = WebControls.WebControls.GetMethod(
                    String.Format("http://www.itsbeta.com/s/{1}/{2}/achieves/posttofbonce.json?access_token={0}&user_id={3}&user_token={4}&activation_code={5}",
                    access_token, category, project, user_id, user_token, activation_code));

                jToken = JToken.Parse(_jsonResponse);
            }
        }

    }
}
