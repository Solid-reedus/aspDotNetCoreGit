using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ASPDotNetCrud.Models;

namespace ASPDotNetCrud.Services
{
    public class SessionService
    {
        private IHttpContextAccessor _httpContextAccessor; 

        // Singleton instance
        private static SessionService _instance; 
        // Lock for thread safety
        private static readonly object _lock = new object(); 

        private SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static SessionService GetInstance(IHttpContextAccessor httpContextAccessor)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SessionService(httpContextAccessor);
                    }
                }
            }
            return _instance;
        }

        public enum SessionKeys
        {
            userSession,
            userName,
            userPic
        }

        public void Set<T>(SessionKeys key, T value)
        {
            string keyString = key.ToString();
            string data = JsonConvert.SerializeObject(value);
            _httpContextAccessor.HttpContext.Session.SetString(keyString, data);
        }

        public void Remove(SessionKeys key)
        {
            string keyString = key.ToString();
            _httpContextAccessor.HttpContext.Session.Remove(keyString);
        }

        // this method will get the values from the json and make a new User from it
        // if the data isnt correct it will return null
        public User GetUserFromSession(SessionKeys key)
        {
            //whitelist
            if (!key.HasFlag(SessionKeys.userSession))
            {
                return null;
            }

            string keyString = key.ToString();
            string data = _httpContextAccessor.HttpContext.Session.GetString(keyString);

            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            try
            {
                JObject jsonObject = JObject.Parse(data);
                string name = (string)jsonObject?["name"];
                string pic = (string)jsonObject?["profilePicture"];
                uint id = (uint)jsonObject?["id"];

                if (name == null || id == 0)
                {
                    return null;
                }

                return new User(name, id, pic);
            }
            catch (JsonReaderException)
            {
                return null;
            }
        }

        public T Get<T>(SessionKeys key)
        {
            string keyString = key.ToString();
            string data = _httpContextAccessor.HttpContext.Session.GetString(keyString);
            T result = JsonConvert.DeserializeObject<T>(data);
            return result;
        }

        public bool SessionVariableExists(SessionKeys key)
        {
            string keyString = key.ToString();

            if (_httpContextAccessor.HttpContext.Session.TryGetValue(keyString, out byte[] value))
            {
                return true;
            }

            return false;
        }
    }
}
