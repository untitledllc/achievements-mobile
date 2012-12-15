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
using Achievements.AndroidPlatform.JsonProcessor;
using Newtonsoft.Json.Linq;

namespace Achievements.AndroidPlatform
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

        public Project[] ProductsArray()
        {
            Project[] array = new Project[Count];

            for (int i = 0; i < Count; i++)
            {
                array[i] = new Project()
                {
                    Name = jSonProjects.jToken[i]["name"].Value<string>(),
                    Title = jSonProjects.jToken[i]["title"].Value<string>(),
                    Id = jSonProjects.jToken[i]["id"].Value<string>()
                };
            }

            return array;
        }

        public class Project
        {
            public string Name { get; set; }
            public string Title { get; set; }
            public string Id { get; set; }

            public string Picture { get; set; }
        }
    }

}