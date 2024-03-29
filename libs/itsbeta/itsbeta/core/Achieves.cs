﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json.Linq;
using ItsBeta.Json;

namespace ItsBeta.Core
{
    public class Achieves
    {
        public JSonProcessor.JSonAchieves jSonAchieves;

        public Achieves(string access_token, string player_id, bool isLocaleRu)
        {
            jSonAchieves = new JSonProcessor.JSonAchieves(access_token, player_id, isLocaleRu);
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
                        int bonusesCount = jSonAchieves.jToken[i]["projects"][j]["achievements"][k]["bonuses"].Count();
                        var bonusesArray = new ParentCategory.ParentProject.Achieve.Bonus[bonusesCount];

                        for (int b = 0; b < bonusesCount; b++)
                        {
                            bonusesArray[b] = new ParentCategory.ParentProject.Achieve.Bonus()
                            {
                                Type = jSonAchieves.jToken[i]["projects"][j]["achievements"][k]["bonuses"][b]["bonus_type"].Value<string>(),
                                Description = jSonAchieves.jToken[i]["projects"][j]["achievements"][k]["bonuses"][b]["bonus_desc"].Value<string>()
                            };
                        }

                        List<ParentCategory.ParentProject.Achieve.Bonus> bonusesList = new List<ParentCategory.ParentProject.Achieve.Bonus>();
                        for (int b = 0; b < bonusesArray.Length; b++)
                        {
                            if (bonusesArray[b].Type == "discount")
                            {
                                bonusesList.Add(bonusesArray[b]);
                            }
                        }
                        for (int b = 0; b < bonusesArray.Length; b++)
                        {
                            if (bonusesArray[b].Type == "bonus")
                            {
                                bonusesList.Add(bonusesArray[b]);
                            }
                        }
                        for (int b = 0; b < bonusesArray.Length; b++)
                        {
                            if (bonusesArray[b].Type == "present")
                            {
                                bonusesList.Add(bonusesArray[b]);
                            }
                        }
                        bonusesArray = bonusesList.ToArray();
                        

                        achArray[k] = new ParentCategory.ParentProject.Achieve()
                        {
                            ApiName = jSonAchieves.jToken[i]["projects"][j]["achievements"][k]["api_name"].Value<string>(),
                            DisplayName = jSonAchieves.jToken[i]["projects"][j]["achievements"][k]["display_name"].Value<string>(),
                            Description = jSonAchieves.jToken[i]["projects"][j]["achievements"][k]["desc"].Value<string>(),
                            PicUrl = jSonAchieves.jToken[i]["projects"][j]["achievements"][k]["pic"].Value<string>(),
                            FbId = jSonAchieves.jToken[i]["projects"][j]["achievements"][k]["fb_id"].Value<string>(),
                            CreateTime = jSonAchieves.jToken[i]["projects"][j]["achievements"][k]["create_time"].Value<string>(),
                            Details = jSonAchieves.jToken[i]["projects"][j]["achievements"][k]["details"].Value<string>(),
                            Advertisments = jSonAchieves.jToken[i]["projects"][j]["achievements"][k]["adv"].Value<string>(),
                            Color = jSonAchieves.jToken[i]["projects"][j]["color"].Value<string>(),
                            Bonuses = bonusesArray
                            //BonusStatus = jSonAchieves.jToken[i]["projects"][j]["achievements"][k]["bonuses"].Value<string>()
                        };
                    }

                    pArray[j] = new ParentCategory.ParentProject()
                    {
                        ApiName = jSonAchieves.jToken[i]["projects"][j]["api_name"].Value<string>(),
                        DisplayName = jSonAchieves.jToken[i]["projects"][j]["display_name"].Value<string>(),
                        Achievements = achArray,
                        TotalBadges = int.Parse(jSonAchieves.jToken[i]["projects"][j]["total_badges"].Value<string>())
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
                public int TotalBadges { get; set; }

                public Achieve[] Achievements { get; set; }
                

                public class Achieve
                {
                    public string ApiName { get; set; }
                    public string DisplayName { get; set; }
                    public string Description { get; set; }
                    public string Details { get; set; }
                    public string Advertisments { get; set; }
                    public string PicUrl { get; set; }
                    public string FbId { get; set; }
                    public string CreateTime { get; set; }
                    public Bonus[] Bonuses { get; set; }
                    public string Color { get; set; }

                    public class Bonus
                    {
                        public string Type { get; set; }
                        public string Description { get; set; } 
                    }
                }
            }
        }

    }
}
