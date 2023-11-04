using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Microsoft.AspNetCore.Session;
using System.Net.Http;
using ASPDotNetCrud.Models;
using Newtonsoft.Json.Linq;

namespace ASPDotNetCrud.Utility
{
    public static class SessionUtility
    {

        public enum SessionKeys
        {
            userSession,
            userName,
            userPic
        }


        public static void Set<T>(SessionKeys key, T value, HttpContext httpContext)
        {
            string keyString = key.ToString();
            string data = JsonConvert.SerializeObject(value);
            httpContext.Session.SetString(keyString, data);
        }

        public static void Remove(SessionKeys key, HttpContext httpContext)
        {
            string keyString = key.ToString();
            httpContext.Session.Remove(keyString);
        }


        public static User GetUserFromSession(SessionKeys key, HttpContext httpContext)
        {

            //whitelist
            if (!(key.HasFlag(SessionKeys.userSession)))
            {
                return new User();
            }

            string keyString = key.ToString();
            string? data = httpContext.Session.GetString(keyString);
            JObject jsonObject = JObject.Parse(data);

            string name = (string)jsonObject?["name"];
            string pic = (string)jsonObject?["profilePicture"];
            uint id = (uint)jsonObject?["id"];

            if (name == null || id == null)
            {
                return new User();
            }

            return new User(name, id, pic);

        }

        public static T Get<T>(SessionKeys key, HttpContext httpContext)
        {
            string keyString = key.ToString();
            string data = httpContext.Session.GetString(keyString);
            T result = JsonConvert.DeserializeObject<T>(data);

            //var user = JsonConvert.DeserializeObject<User>(yourJsonString, new UserConverter());

            //T result = JsonConvert.DeserializeObject<T>(data);

            return result;
        }

        public static bool SessionVariableExists(SessionKeys key, HttpContext httpContext)
        {
            string keyString = key.ToString();

            if (httpContext.Session.TryGetValue(keyString, out byte[] value))
            {
                // The session variable with the specified key exists.
                return true;
            }

            // The session variable does not exist.
            return false;
        }



    }
}
