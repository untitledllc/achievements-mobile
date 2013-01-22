using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json.Linq;
using ItsBeta.Json;

namespace ItsBeta.Core
{
    public class Objs
    {
        JSonProcessor.JSonObj jSonObj;

        public Objs(string access_token, string objtemplate_id, string player_id)
        {
            jSonObj = new JSonProcessor.JSonObj(access_token, objtemplate_id, player_id);
        }

        public int Count
        {
            get
            {
                return jSonObj.Count;
            }
            private set { }
        }

        public Obj[] ObjTemplatesArray()
        {
            Obj[] array = new Obj[Count];

            for (int i = 0; i < Count; i++)
            {
                array[i] = new Obj()
                {
                    Id = jSonObj.jToken[i]["id"].Value<string>(),
                    ApiName = jSonObj.jToken[i]["api_name"].Value<string>()
                    //Дописать MyExtParams
                };
            }

            return array;
        }


        public class Obj
        {
            public string Id { get; set; }
            public string ApiName { get; set; }


            public class MyExtParam
            {
                public string Pname { get; set; }
                public string Pvalue { get; set; }
            }
        }
    }
}
