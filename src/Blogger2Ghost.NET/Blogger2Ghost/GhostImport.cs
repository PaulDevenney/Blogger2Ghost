using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogger2Ghost
{

    public class Meta
    {
        public long exported_on { get; set; }
        public string version { get; set; }
    }

    public class Post
    {
        public int id { get; set; }
        public string title { get; set; }
        public string slug { get; set; }
        public string markdown { get; set; }
        public string html { get; set; }
        public object image { get; set; }
        public int featured { get; set; }
        public int page { get; set; }
        public string status { get; set; }
        public string language { get; set; }
        public object meta_title { get; set; }
        public object meta_description { get; set; }
        public int author_id { get; set; }
        public object created_at { get; set; }
        public int created_by { get; set; }
        public object updated_at { get; set; }
        public int updated_by { get; set; }
        public object published_at { get; set; }
        public int published_by { get; set; }
    }

    public class Tag
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string description { get; set; }
    }

    public class PostsTag
    {
        public int tag_id { get; set; }
        public int post_id { get; set; }
    }

    public class Data
    {
        public Data()
        {
            posts = new List<Post>();
            tags = new List<Tag>();
            posts_tags = new List<PostsTag>();
        }

        public List<Post> posts { get; set; }
        public List<Tag> tags { get; set; }
        public List<PostsTag> posts_tags { get; set; }
    }

    public class Db
    {
        public Db()
        {
            data = new Data();
        }
        public Meta meta { get; set; }
        public Data data { get; set; }
    }

    public class GhostImport
    {
        public GhostImport()
        {
            db = new List<Db>();
        }
        public List<Db> db { get; set; }
    }

}
