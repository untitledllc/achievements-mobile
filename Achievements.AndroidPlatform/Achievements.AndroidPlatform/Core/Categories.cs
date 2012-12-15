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
    public class Categories
    {
        JSonProcessor.JSonCategories jSonCategories;

        public Categories(string access_token)
        {
            jSonCategories = new JSonProcessor.JSonCategories(access_token);
        }

        public int Count 
        {
            get 
            {
                return jSonCategories.Count;
            }
            private set { }
        }

        public Category[] CategoriesArray()
        {
            Category[] array = new Category[Count];

            for (int i = 0; i < Count; i++)
            {
                array[i] = new Category()
                {
                    Name = jSonCategories.jToken[i]["name"].Value<string>(),
                    Title = jSonCategories.jToken[i]["title"].Value<string>(),
                    Id = jSonCategories.jToken[i]["id"].Value<string>()
                };
            }

            return array;
        }

        public class Category
        {
            public string Name { get; set; }
            public string Id { get; set; }
            public string Title { get; set; }

            public ImageView Picture { get; set; }
        }
    }
}