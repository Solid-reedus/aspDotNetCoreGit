﻿namespace ASPDotNetCrud.Models
{
    public class Post
    {
        Post(string _title, uint _id)
        {
            title = _title;
            id = _id;
            subTitle = null;
            score = 0;
            comments = new List<Post> { this };
        }

        public Post(string _title, uint _id, string? _subTitle, string? _picture, uint _postUser, uint _community)
        {
            title = _title;
            id = _id;

            if (_subTitle != null)
            {
                subTitle = _subTitle;
            }
            else
            {
                subTitle = null;
            }

            if (_picture != null)
            {
                picture = _picture;
            }
            else
            {
                picture = null;
            }

            score = 0;
            comments = new List<Post> { this };
            postUser = _postUser;
            community = _community;
        }

        public List<Post> GetComments()
        {
            return comments;
        }

        public string? GetSubTitle()
        {
            if (subTitle != null)
            {
                return subTitle;
            }
            else 
            { 
                return null; 
            }
        }

        public string title { get; private set; }

        public uint id { get; private set; }
        public uint postUser { get; private set; }
        public uint community { get; private set; }

        public string? picture { get; private set; }

        //comments
        public List<Post> comments { get; private set; }

        public long score { get; set; }
        public string? subTitle { get; set; }


    }
}
