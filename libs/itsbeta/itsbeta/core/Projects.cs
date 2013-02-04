using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json.Linq;
using ItsBeta.Json;

namespace ItsBeta.Core
{
    public class Projects
    {
        JSonProcessor.JSonProjects jSonProjects;

        public Projects(string access_token, string categoryId)
        {
            jSonProjects = new JSonProcessor.JSonProjects(access_token, categoryId);
        }

        public int Count
        {
            get
            {
                return jSonProjects.Count;
            }
            private set { }
        }

        public Project[] ProjectsArray()
        {
            Project[] array = new Project[Count];

            for (int i = 0; i < Count; i++)
            {
                array[i] = new Project()
                {
                    Name = jSonProjects.jToken[i]["api_name"].Value<string>(),
                    Title = jSonProjects.jToken[i]["display_name"].Value<string>(),
                    Id = jSonProjects.jToken[i]["id"].Value<string>(),
                    CategoryName = jSonProjects.jToken[i]["category_name"].Value<string>()
                };
            }

            return array;
        }

        public class Project
        {
            public string Name { get; set; }
            public string Title { get; set; }
            public string Id { get; set; } //Id in database
            public string CategoryName { get; set; }
        }
    }

}