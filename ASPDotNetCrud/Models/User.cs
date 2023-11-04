using System.Security.Cryptography;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ASPDotNetCrud.Models
{
    [JsonConverter(typeof(User))]
    public class User : JsonConverter<User>
    {

        public override bool CanWrite => true;
        public override bool CanRead => false;

        public override void WriteJson(JsonWriter writer, User value, JsonSerializer serializer)
        {
            JObject jsonObject = new JObject();
            jsonObject.Add("name", value.name);
            jsonObject.Add("profilePicture", value.profilePicture);
            jsonObject.Add("id", value.id);

            jsonObject.WriteTo(writer);
        }

        public override User ReadJson(JsonReader reader, Type objectType, User existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            User user = new User(); // Create an instance of User.

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    string propertyName = reader.Value.ToString();

                    if (propertyName.Equals("name", StringComparison.OrdinalIgnoreCase))
                    {
                        reader.Read(); // Move to the property value.
                        user.name = (string)reader.Value;
                    }
                    else if (propertyName.Equals("profilePicture", StringComparison.OrdinalIgnoreCase))
                    {
                        reader.Read(); // Move to the property value.
                        user.profilePicture = (string)reader.Value;
                    }
                    else if (propertyName.Equals("id", StringComparison.OrdinalIgnoreCase))
                    {
                        reader.Read(); // Move to the property value.
                        user.id = (uint)reader.Value;
                    }
                }
            }

            return user;
        }

        public User() 
        {
            name = "";
            id = 0;
            profilePicture = null;
        }
        public User(string _name, uint _id)
        {
            name = _name;
            id = _id;
            profilePicture = null;
        }

        public User(string _name, uint _id, string _profilePicture)
        {
            name = _name;
            id = _id;
            profilePicture = _profilePicture;
        }

        public string name { get; private set; }
        public string? profilePicture { get; private set; }
        uint id { get; set; }

    }
}
