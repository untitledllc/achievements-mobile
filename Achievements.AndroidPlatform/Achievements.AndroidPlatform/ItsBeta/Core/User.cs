using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json.Linq;
using ItsBeta.Json;

namespace Achievements.AndroidPlatform.ItsBeta.Core
{
    public class User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Fullname { get; set; }
        public string Gender { get; set; }
        
        public string Nick { get; set; }

        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }

        public string Locale { get; set; }

        //birthday
        public string BirthDate { get; set; }

        public string FacebookUserID { get; set; }

        public string ItsBetaUserId { get; set; }
    }
}
