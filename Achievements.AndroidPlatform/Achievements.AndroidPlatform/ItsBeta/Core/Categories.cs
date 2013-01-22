using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json.Linq;
using ItsBeta.Json;

namespace ItsBeta.Core
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
                    Name = jSonCategories.jToken[i]["api_name"].Value<string>(),
                    Title = jSonCategories.jToken[i]["display_name"].Value<string>(),
                };
            }

            return array;
        }

        public class Category
        {
            public string Name { get; set; } //API NAME
            public string Title { get; set; } //Readable Name
        }
    }
}