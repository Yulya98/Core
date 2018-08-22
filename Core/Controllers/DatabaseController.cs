using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Core.Controllers
{
    [Route("api/[controller]")]
    public class DatabaseController : Controller
    {
        [Route("searchphoto")]
        public string[] SearchPhoto(Newtonsoft.Json.Linq.JObject idUser)
        {
            string userId = HttpContext.Session.Get("userId").ToString();
            string[] array = WorkWithDb.PathPhoto(userId, Convert.ToInt32(idUser["idAlbum"])).ToArray();
            return array;
        }

        [Route("addPhoto")]
        public void AddPhoto(Newtonsoft.Json.Linq.JObject photo)
        {
            string userId = HttpContext.Session.Get("userId").ToString();
            WorkWithDb.AddNewPhoto(photo["NameImg"].ToString(), photo["Path"].ToString(), Convert.ToInt32(photo["idAlbum"]), userId);
        }

        [Route("searchPosts")]
        public string[] searchImagesPosts()
        {
            string[] posts = WorkWithDb.SearchPosts().ToArray();
            return posts;
        }

        [Route("searchComments")]
        public string[] SearchCommentsToPosts(Newtonsoft.Json.Linq.JObject idPost)
        {
            string[] comments = WorkWithDb.SearchComments(Convert.ToInt32(idPost["idPost"])).ToArray();
            return comments;
        }

        [Route("searchAuthor")]
        public string[] AuthorNameAndImagePath(Newtonsoft.Json.Linq.JObject idPost)
        {
            string[] authorsPaths = WorkWithDb.AuthorNameAndImagePath(Convert.ToInt32(idPost["idPost"])).ToArray();
            return authorsPaths;
        }

        [Route("photoDelete")]
        public void PhotoDelete(Newtonsoft.Json.Linq.JObject photo)
        {
            string userId = HttpContext.Session.Get("userId").ToString();
            WorkWithDb.DeletePhoto(photo["nameImg"].ToString(), Convert.ToInt32(photo["idAlbum"]), userId);
        }

        [Route("addComment")]
        public void AddComment(Newtonsoft.Json.Linq.JObject photo)
        {
            WorkWithDb.AddComment(Convert.ToInt32(photo["idPost"]), photo["text"].ToString());
        }

        //[Route("defineRegistrationUser")]
        //public bool DefineRegistrationUser(Newtonsoft.Json.Linq.JObject photo)
        //{
        //    string userId = HttpContext.Session.Get("userId").ToString();
        //    if (userId != -1)
        //        return true;
        //    else
        //        return false;
        //}


        [Route("addAlbum")]
        public int AddAlbum(Newtonsoft.Json.Linq.JObject album)
        {
            int id = WorkWithDb.AddAlbum(album["userId"].ToString(), album["nameOfAlbum"].ToString());
            return id;
        }

        [Route("searchAlbum")]
        public string[] SearchAlbum(Newtonsoft.Json.Linq.JObject album)
        {
            string[] albums = WorkWithDb.SearchAlbum(album["userId"].ToString()).ToArray();
            return albums;
        }
    }
}