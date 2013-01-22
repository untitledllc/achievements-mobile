using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json.Linq;
using ItsBeta.Json;

namespace ItsBeta.Core
{
    public class ObjTemplates
    {
        JSonProcessor.JSonObjTemplates jSonObjTemplates;

        public ObjTemplates(string access_token, string objtype_id)
        {
            jSonObjTemplates = new JSonProcessor.JSonObjTemplates(access_token, objtype_id);
        }

        public ObjTemplates(string access_token, string objtype_id, string project_id)
        {
            jSonObjTemplates = new JSonProcessor.JSonObjTemplates(access_token, objtype_id, project_id);
        }

        public int Count
        {
            get
            {
                return jSonObjTemplates.Count;
            }
            private set { }
        }

        public ObjTemplate[] ObjTemplatesArray()
        {
            ObjTemplate[] array = new ObjTemplate[Count];

            for (int i = 0; i < Count; i++)
            {
                array[i] = new ObjTemplate()
                {
                    Id = jSonObjTemplates.jToken[i]["id"].Value<string>(),
                    ApiName = jSonObjTemplates.jToken[i]["api_name"].Value<string>(),
                    ProjectId = jSonObjTemplates.jToken[i]["project_id"].Value<string>(),
                    ObjTypeId = jSonObjTemplates.jToken[i]["objtype_id"].Value<string>(),
                    PicUrl = jSonObjTemplates.jToken[i]["pic"].Value<string>(),
                    //Дописать MyIntParams
                };
            }

            return array;
        }


        public class ObjTemplate
        {
            public string Id { get; set; }
            public string ApiName { get; set; }
            public string ObjTypeId { get; set; }
            public string ProjectId { get; set; }
            public string PicUrl { get; set; }


            public class MyIntParam
            {
                public string Pname { get; set; }
                public string Pvalue { get; set; }
            }

            public class MyShrParam
            {
                public string Pname { get; set; }
                public string Pvalue { get; set; }
            }
        }
    }
}
