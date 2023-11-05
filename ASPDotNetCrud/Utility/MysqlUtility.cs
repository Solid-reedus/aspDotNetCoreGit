﻿using ASPDotNetCrud.Models;
using MySqlConnector;
using System.Xml.Linq;

namespace ASPDotNetCrud.Utility
{
    public static class MysqlUtility
    {
        public enum UserProperties
        {
            name,
            password,
            profilepic
        }

        private static MySqlConnection GetConnection()
        {
            return new MySqlConnection("Server=localhost;User ID=root;Password=;Database=aspdotnetcrud_db");
        }



        public static List<Community> GetCommunities()
        {
            List<Community> communities = new List<Community>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string request = "SELECT * FROM communities";
                MySqlCommand cmd = new MySqlCommand(request, conn);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        uint id = reader.GetUInt32("community_id"); 
                        string name = reader.GetString("community_name"); 
                        string description = reader.GetString("community_description"); 
                        uint owner = reader.GetUInt32("community_owner");

                        communities.Add(new Community(id, name, description, owner));
                    }
                }
            }

            return communities;
        }

        public static bool UserNameIsTaken(string _name)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                //SELECT user_name FROM `users` WHERE user_name = "jeff";
                string request = "SELECT user_name FROM `users` WHERE user_name = \" " + _name + " \" ";
                MySqlCommand cmd = new MySqlCommand(request, conn);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        public static bool UpdateUser(uint who, UserProperties what, string newValue)
        {
            string target = "";
            switch (what)
            {
                case UserProperties.name:
                    target = "user_name";
                    break;
                case UserProperties.password:
                    target = "user_password";
                    break;
                case UserProperties.profilepic:
                    target = "user_prof_pic_route";
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(target))
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    string request = $"UPDATE users SET {target} = @newValue WHERE user_id = @who";

                    MySqlCommand cmd = new MySqlCommand(request, conn);

                    cmd.Parameters.AddWithValue("@newValue", newValue);
                    cmd.Parameters.AddWithValue("@who", who);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }

            return false;
        }

        public static bool UserExists(string _name, string _password)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string request = "SELECT * FROM users WHERE user_name = @UserName AND user_password = @Password";
                MySqlCommand cmd = new MySqlCommand(request, conn);
                cmd.Parameters.AddWithValue("@UserName", _name);
                cmd.Parameters.AddWithValue("@Password", _password); // Hash the input password

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }


        public static bool InsertUser(string name, string password, string? profilePicture = null)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string request = "INSERT INTO users (user_name, user_password) " +
                                 "VALUES (@name, @password)";
                MySqlCommand cmd = new MySqlCommand(request, conn);

                // You should hash the password before storing it for security reasons.
                // Here, we assume that the password is already hashed.
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@password", password);

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }


        public static User getUser(string _name, string _password)
        {
            User user = new User();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string request = $"SELECT * FROM users WHERE user_name = '{_name}' AND user_password = '{_password}'";
                MySqlCommand cmd = new MySqlCommand(request, conn);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        uint id = reader.GetUInt32("user_id");
                        string name = reader.GetString("user_name");
                        string prfPic = reader.GetString("user_prof_pic_route");
                        user = new User(name, id, prfPic);
                    }
                }
            }
            return user;
        }
    }
}
