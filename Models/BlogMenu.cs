using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using System.ComponentModel.DataAnnotations;

namespace BlogsConsole.Models
{
    class BlogMenu
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void Process(int selection)
        {
            BlogMenu bm = new BlogMenu();


            //bm.TicketMenuHeader();
            //f.ShowTickets();
            //bm.ViewTicketMenu();
            //selection = bm.GetInput();

            switch (selection)
            {
                case 1:
                    bm.ViewBlogs();
                    break;
                case 2:
                    bm.AddBlog();
                    break;
                case 3:
                    bm.CreatePost();
                    break;
                case 4:
                    bm.DisplayPosts();
                    break;
                case 5:
                    bm.DeletePost();
                    break;
                case 6:
                    bm.EditPost();
                    break;
            }
        }



        public void ViewBlogs()
        {
            Format f = new Format();
            var db = new BloggingContext();
            var query = db.Blogs.OrderBy(b => b.BlogId);


            Console.Clear();
            f.ViewBlogsHeader();

            //Console.WriteLine($"    Matt has {query.Count()} Blogs\n");

                foreach (var item in query)
                {
                    Console.WriteLine(f.ViewBlogsFormat(), item.BlogId, item.Name);
                }
            Console.Write("\n    Press any key to continue: ");
            Console.ReadKey();
        }

        public void AddBlog()
        {
            // Add blog
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n    ---------------------------------------------------------------------------------------------\n" +
                "    Add Blog\n" +
                "    ---------------------------------------------------------------------------------------------\n");
            Console.ResetColor();
            Console.Write("    Enter a name for a new Blog: ");

            var blog = new Blog { Name = Console.ReadLine() };

            ValidationContext context = new ValidationContext(blog, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var db = new BloggingContext();
            do
            {
                if (db.Blogs.Any(b => b.Name == blog.Name))
                {
                    foreach (var result in results)
                    {
                        logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                    }
                    Console.Write("\n    This Blog Name Already Exists. Please enter a unique Blog Name: ");
                    blog = new Blog { Name = Console.ReadLine() };
                }
            } while (db.Blogs.Any(b => b.Name == blog.Name));

            db.AddBlog(blog);
            //logger.Info("Blog added - {name}", blog.Name);
            Console.WriteLine($"\n    Blog {blog.Name} successfully created!");
            Console.Write("\n    Press any key to continue: ");
            Console.ReadKey();
        }

        public void CreatePost()
        {
            Format f = new Format();
            var db = new BloggingContext();
            var query = db.Blogs.OrderBy(b => b.BlogId);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n    ---------------------------------------------------------------------------------------------\n" +
                "    Create Blog Post\n" +
                "    ---------------------------------------------------------------------------------------------\n");

            f.ViewBlogsSubHeader();
            foreach (var item in query)
            {
                Console.WriteLine(f.ViewBlogsFormat(), item.BlogId, item.Name);
            }
            Console.Write("    Choose a Blog ID to create a Post: \n");

            int blogid = f.validateInt(Console.ReadLine());
            int i = 0;
            do
            {
                if (db.Blogs.Any(b => b.BlogId == blogid))
                {
                    Post post = AddPost(db);
                    if (post != null)
                    {
                        post.BlogId = blogid;
                        db.AddPost(post);
                        Console.WriteLine($"\n    Post successfully created!");
                        i = 1;
                        //logger.Info("Post added - {title}", post.Title);
                    }
                }
                else if (db.Blogs.Any(b => b.BlogId != blogid))
                {
                    //logger.Error("There are no Blogs saved with that Id");
                    Console.WriteLine("    Please choose a valid Blog ID to create a Post: \n");
                    blogid = f.validateInt(Console.ReadLine());
                }
            } while (i == 0);
            Console.Write("\n    Press any key to continue: ");
            Console.ReadKey();
        }

        public static Post AddPost(BloggingContext db)
        {
            Post post = new Post();
            Console.Write("    Enter the Post title: ");
            post.Title = Console.ReadLine();
            Console.Write("    Enter the Post content: ");
            post.Content = Console.ReadLine();

            ValidationContext context = new ValidationContext(post, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(post, context, results, true);
            if (isValid)
            {
                return post;
            }
            else
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
            return null;
        }

        public void DisplayPosts()
        {
            // Display Posts
            var db = new BloggingContext();
            Format f = new Format();
            var query = db.Blogs.OrderBy(b => b.BlogId);

            Console.Clear();
            f.ViewBlogsHeader();

            IEnumerable<Post> Posts;

            foreach (var item in query)
            {
                Console.WriteLine(f.ViewBlogsFormat(),item.BlogId,item.Name);
            }
            Console.Write("\n    Select the Blog ID to display its Posts: ");

            int blogid = f.validateInt(Console.ReadLine());
            int i = 0;
            
            do
            {
                if (db.Blogs.Count(b => b.BlogId == blogid) > 0)
                {
                    // display posts from all blogs
                    Posts = db.Posts.OrderBy(p => p.Title);
                    // display post from selected blog
                    Posts = db.Posts.Where(p => p.BlogId == blogid).OrderBy(p => p.Title);

                    if (Posts.Count() == 0)
                    {
                        Console.WriteLine("    Sorry, this blog has no posts...");
                    }
                    else
                    {
                        Console.WriteLine($"\n    {Posts.Count()} post(s) returned:\n ");
                        foreach (var item in Posts)
                        {
                            Console.WriteLine($"    Post Title: {item.Title}\n    Content: {item.Content}\n");
                        }
                    }
                    i = 1;
                }
                else if (blogid != 0 && db.Blogs.Count(b => b.BlogId == blogid) == 0)
                {
                    //logger.Error("There are no Blogs saved with that Id");
                    Console.WriteLine("    Please choose a valid Blog ID display its Posts: \n");
                    blogid = f.validateInt(Console.ReadLine());
                }
            } while (i == 0);
            Console.Write("\n    Press any key to continue: ");
            Console.ReadKey();
        }



        public void EditPost()
        {
            Format f = new Format();
            var db = new BloggingContext();
            var query = db.Blogs.OrderBy(b => b.BlogId);

            Console.Clear();
            f.EditPostHeader();

            var post = ShowPost(db);
            if (post != null)
            {
                Post UpdatedPost = InputPost(db);
                if (UpdatedPost != null)
                {
                    UpdatedPost.PostId = post.PostId;
                    db.EditPost(UpdatedPost);
                    Console.WriteLine("    Post Successfully Updated!");
                    // logger.Info("Post (id: {postid}) updated", UpdatedPost.PostId);
                }
            }
            Console.Write("\n    Press any key to continue: ");
            Console.ReadKey();
        }

        public void DeletePost()
        {
            Format f = new Format();
            var db = new BloggingContext();
            var query = db.Blogs.OrderBy(b => b.BlogId);

            Console.Clear();
            f.DeletePostHeader();
            var post = ShowPost(db);
            
            if (post != null)
            {
                db.DeletePost(post);
                Console.Write("\n    Post Successfully deleted! ");
                //logger.Info("Post (id: {postid}) deleted", post.PostId);
            }
            Console.Write("\n    Press any key to continue: ");
            Console.ReadKey();
        }



        public static Post InputPost(BloggingContext db)
        {
            Post post = new Post();
            Console.WriteLine("    Enter the Post title");
            post.Title = Console.ReadLine();
            Console.WriteLine("    Enter the Post content");
            post.Content = Console.ReadLine();

            ValidationContext context = new ValidationContext(post, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(post, context, results, true);
            if (isValid)
            {
                return post;
            }
            else
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
            return null;
        }


        public static Post ShowPost(BloggingContext db)
        {
            Format f = new Format();

            var blogs = db.Blogs.Include("Posts").OrderBy(b => b.BlogId);
            foreach (Blog b in blogs)
            {
                //Console.WriteLine(b.Name);
                if (b.Posts.Count() > 0)
                {
                    foreach (Post p in b.Posts)
                    {
                        Console.WriteLine(f.ViewBlogsFormat(), p.PostId, p.Title);
                    }
                }
            }
            Console.WriteLine("\n    Choose the post ID:");
            int postid = f.validateInt(Console.ReadLine());
            do
            {
                if (db.Posts.Any(b => b.PostId == postid))
                {
                    Post post = db.Posts.FirstOrDefault(p => p.PostId == postid);
                    return post;
                }
                else if (db.Posts.Any(b => b.PostId != postid))
                {
                    //logger.Error("There are no Blogs saved with that Id");
                    Console.WriteLine("    Please choose a valid Post ID, or enter 0 to cancel: \n");
                    postid = f.validateIntZero(Console.ReadLine());
                }
            } while ((postid != 0) && (db.Posts.Any(b => b.PostId != postid)));

            return null;
        }


        public int GetInput()
        {
            Format i = new Format();
            int selection;

            selection = i.validateInt(Console.ReadLine());

            while ((selection < 0 || selection > 7))
            {
                Console.Write("    Please Enter a valid response 1 - 7 ");
                selection = i.validateInt(Console.ReadLine());
            }
            return selection;
        }

    }
}
