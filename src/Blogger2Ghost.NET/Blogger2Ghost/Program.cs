using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using Slugify;

namespace Blogger2Ghost
{
    class Program
    {
        private static SlugHelper _slugifyHelper;

        static void Main(string[] args)
        {
#if DEBUG
            Console.WriteLine("Attach debugger now, then press key to continue");
            Console.ReadKey();
#endif

            if (args.Length != 2)
            {
                Console.WriteLine("Usage: Blogger2Ghost <bloggerexport.xml> <ghostimport.json>");
            }

            var bloggerXmlFile = args[0]; //@"C:\Users\Paul\Downloads\blog-04-23-2016.xml"
            var ghostOutputFile = args[1]; //ghost.json

            var slugConfig = new SlugHelper.Config
            {
                ForceLowerCase = true,
                CharacterReplacements = new Dictionary<string, string>()
                {
                    {" ", "_"},
                    { "'", "_"},
                    {"&", "_"},
                    {"-", "_"}
                }
            };

            _slugifyHelper = new SlugHelper(slugConfig);
            var masterTerms = new List<string>();


            var root = XElement.Load(bloggerXmlFile);

            var ghostImport = new GhostImport()
            {
                db = new List<Db>()
                {
                    new Db()
                    {
                        meta = new Meta()
                        {
                            version = "004",
                            exported_on = DateTime.UtcNow.ToEpochTimeInMilliseconds()
                        }
                    }
                }
            };

            int postId = 0;
            foreach (var element in root.Elements().Where(e => e.Name.LocalName == "entry"))
            {
                var category = element.Elements().Single(e => e.Name.LocalName == "category" && e.Attribute("scheme").Value == "http://schemas.google.com/g/2005#kind");
                var terms = element.Elements().Where(e => e.Name.LocalName == "category" && e.Attribute("scheme").Value == "http://www.blogger.com/atom/ns#");

                //add any new terms
                foreach (var termValue in terms.Select(term => term.Attribute("term").Value).Where(termValue => !masterTerms.Contains(termValue)))
                {
                    masterTerms.Add(termValue);
                }

                //It's a post
                if (category.Attribute("term").Value == "http://schemas.google.com/blogger/2008/kind#post")
                {
                    var post = CreatePostFromElement(postId, element);
                    if (String.IsNullOrEmpty(post.slug))
                    {
                        post.slug = "Untitled_" + postId;
                    }
                    if (String.IsNullOrEmpty(post.title))
                    {
                        post.title = "Untitled " + postId;
                    }

                    ghostImport.db[0].data.posts.Add(post);

                    postId++;
                }
            }

            var json = JsonConvert.SerializeObject(ghostImport);
            File.WriteAllText(ghostOutputFile, json);
        }

        private static Post CreatePostFromElement(int postId, XElement element)
        {
            //test for draft status
            var status = GetPublishingStatus(element);

            var title = element.Elements().Single(e => e.Name.LocalName == "title").Value;
            var published = element.Elements().Single(e => e.Name.LocalName == "published").Value;
            var updated = element.Elements().Single(e => e.Name.LocalName == "updated").Value;
            var content = element.Elements().Single(e => e.Name.LocalName == "content").Value;

            var publishedEpoch = DateTime.Parse(published).ToEpochTimeInMilliseconds();
            var updatedEpoch = DateTime.Parse(updated).ToEpochTimeInMilliseconds();

            var slug = _slugifyHelper.GenerateSlug(title);

            var post = new Post()
            {
                id = postId,
                title = title,
                slug = slug,
                status = status.ToString().ToLower(),
                markdown = content,
                html = content,

                published_at = publishedEpoch,
                created_at = publishedEpoch,
                updated_at = updatedEpoch,
                created_by = 1,
                author_id = 1,
                published_by = 1,
                updated_by = 1,

                featured = 0,
                page = 0,
                image = null,
                language = "en_US",
                meta_title = null,
                meta_description = null
            };

            return post;
        }

        private static BlogStatus GetPublishingStatus(XElement element)
        {
            var status = BlogStatus.Published;
            var control = element.Elements().FirstOrDefault(e => e.Name.LocalName == "control");
            var draft = control?.Elements().FirstOrDefault(e => e.Name.LocalName == "draft");

            if (draft != null && draft.Value == "yes")
            {
                status = BlogStatus.Draft;
            }
            return status;
        }
    }

}
