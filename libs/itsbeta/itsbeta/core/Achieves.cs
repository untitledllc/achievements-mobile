using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json.Linq;
using ItsBeta.Json;

namespace ItsBeta.Core
{
    public class Achieves
    {
        JSonProcessor.JSonAchieves jSonAchieves;

        public Achieves(string access_token, string player_id)
        {
            jSonAchieves = new JSonProcessor.JSonAchieves(access_token, player_id);
        }

        public int CategoriesCount
        {
            get
            {
                return jSonAchieves.Count;
            }
            private set { }
        }

        public ParentCategory[] CategoryArray 
        {
            get
            {
                return ParentCategoryArray();
            }
            private set { }
        }

        ParentCategory[] ParentCategoryArray()
        {
            ParentCategory[] array = new ParentCategory[CategoriesCount];
            for (int i = 0; i < CategoriesCount; i++)
            {
                int projectsCount = jSonAchieves.jToken[i]["projects"].Count();
                var pArray = new ParentCategory.ParentProject[projectsCount];

                for (int j = 0; j < projectsCount; j++)
                {
                    int achievesCount = jSonAchieves.jToken[i]["projects"][j]["achievements"].Count();
                    var achArray = new ParentCategory.ParentProject.Achieve[achievesCount];

                    for (int k = 0; k < achievesCount; k++) 
                    {
                        achArray[k] = new ParentCategory.ParentProject.Achieve()
                        {
                            ApiName = jSonAchieves.jToken[i]["projects"][j]["achievements"][k]["api_name"].Value<string>(),
                            DisplayName = jSonAchieves.jToken[i]["projects"][j]["achievements"][k]["display_name"].Value<string>(),
                            Description = jSonAchieves.jToken[i]["projects"][j]["achievements"][k]["desc"].Value<string>(),
                            PicUrl = jSonAchieves.jToken[i]["projects"][j]["achievements"][k]["pic"].Value<string>(),
                            FbId = jSonAchieves.jToken[i]["projects"][j]["achievements"][k]["fb_id"].Value<string>(),
                            CreateTime = jSonAchieves.jToken[i]["projects"][j]["achievements"][k]["create_time"].Value<string>(),
                            BonusStatus = jSonAchieves.jToken[i]["projects"][j]["achievements"][k]["bonus"].Value<string>()
                        };
                    }

                    pArray[j] = new ParentCategory.ParentProject()
                    {
                        ApiName = jSonAchieves.jToken[i]["projects"][j]["api_name"].Value<string>(),
                        DisplayName = jSonAchieves.jToken[i]["projects"][j]["display_name"].Value<string>(),
                        Achievements = achArray
                    };
                }

                array[i] = new ParentCategory()
                {
                    ApiName = jSonAchieves.jToken[i]["api_name"].Value<string>(),
                    DisplayName = jSonAchieves.jToken[i]["display_name"].Value<string>(),
                    Projects = pArray
                };
            }

            return array;
        }

        public class ParentCategory
        {
            public string ApiName { get; set; }
            public string DisplayName { get; set; }

            public ParentProject[] Projects { get; set; }

            public class ParentProject
            {
                public string ApiName { get; set; }
                public string DisplayName { get; set; }

                public Achieve[] Achievements { get; set; }

                public class Achieve
                {
                    public string ApiName { get; set; }
                    public string DisplayName { get; set; }
                    public string Description { get; set; }
                    public string PicUrl { get; set; }
                    public string FbId { get; set; }
                    public string CreateTime { get; set; }
                    public string BonusStatus { get; set; }   
                }
            }
        }


    }
}
