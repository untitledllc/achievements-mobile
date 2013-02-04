using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json.Linq;
using ItsBeta.Json;

namespace ItsBeta.Core
{
    public class ServiceItsBeta
    {
        JSonProcessor.JSonPostToFbOnce jSonPostToFbOnce;
        JSonProcessor.JSonPlayerId jSonPlayerId;

        public ServiceItsBeta()
        {
            
        }

        public string GetItsBetaUserID(string facebookUserID)
        {
            jSonPlayerId = new JSonProcessor.JSonPlayerId("059db4f010c5f40bf4a73a28222dd3e3", "fb_user_id", facebookUserID);
            return jSonPlayerId.jToken["player_id"].Value<string>();
        }

        public void PostToFbOnce(string access_token, string category, string project, string badge_name, string user_id, string user_token)
        {
            jSonPostToFbOnce = new JSonProcessor.JSonPostToFbOnce(access_token, category, project, badge_name, user_id, user_token);
        }

        public bool GetPlayerExistBool(string facebookUserID)
        {
            jSonPlayerId = new JSonProcessor.JSonPlayerId("059db4f010c5f40bf4a73a28222dd3e3", "fb_user_id", facebookUserID);
            
            if (jSonPlayerId.jToken["error"] != null)
	        {
		        return false;
	        }
            return true;
        }
    }
}
