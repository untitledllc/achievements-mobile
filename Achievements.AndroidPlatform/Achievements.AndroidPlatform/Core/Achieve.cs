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

namespace Achievements.AndroidPlatform
{
    public class Achieve
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string Picture { get; set; }
        public DateTime LastUpdateTime { get; set; }

    }
}