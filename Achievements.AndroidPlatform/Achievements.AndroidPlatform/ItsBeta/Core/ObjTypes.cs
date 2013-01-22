using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json.Linq;
using ItsBeta.Json;

namespace ItsBeta.Core
{
    public class ObjTypes
    {
        JSonProcessor.JSonObjTypes jSonObjTypes;

        public ObjTypes(string access_token)
        {
            jSonObjTypes = new JSonProcessor.JSonObjTypes(access_token);
        }

        public ObjTypes(string access_token, string project_id, string parent_id)
        {
            jSonObjTypes = new JSonProcessor.JSonObjTypes(access_token, project_id, parent_id);
        }

        public ObjTypes(string access_token, string unit_id, bool isParentId) //If !isParentId - project_id
        {
            jSonObjTypes = new JSonProcessor.JSonObjTypes(access_token, unit_id, isParentId);
        }

        public int Count
        {
            get
            {
                return jSonObjTypes.Count;
            }
            private set { }
        }


        public ObjType[] ObjTypesArray()
        {
            ObjType[] array = new ObjType[Count];

            for (int i = 0; i < Count; i++)
            {
                array[i] = new ObjType()
                {
                    Id = jSonObjTypes.jToken[i]["id"].Value<string>(),
                    ApiName = jSonObjTypes.jToken[i]["api_name"].Value<string>(),
                    ProjectId = jSonObjTypes.jToken[i]["project_id"].Value<string>(),
                    ParentId = jSonObjTypes.jToken[i]["parent_id"].Value<string>(),
                    //Дописать MyExtParams
                };
            }

            return array;
        }


        public class ObjType
        {
            public string Id { get; set; }
            public string ApiName { get; set; }
            public string ProjectId { get; set; }
            public string ParentId { get; set; }

            public MyExtParam[] MyExtParams { get; set; }
            public MyIntParam[] MyIntParams { get; set; }
            public MyShrParam[] MyShrParams { get; set; } 

            public class MyExtParam
            {
                public string Pname { get; set; }
                public string Ptype { get; set; }
            }

            public class MyIntParam
            {
                public string Pname { get; set; }
                public string Ptype { get; set; }
            }

            public class MyShrParam
            {
                public string Pname { get; set; }
                public string Ptype { get; set; }
            }
            
        }
    }
}
