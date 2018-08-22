using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core
{
    public class WorkWithDb
    {

        public static List<string> PathPhoto(string id, int idAlbum)
        {
            TokenDatabaseContext context = new TokenDatabaseContext();
            Task<int?[]> idPosts = (from posts in context.UserAlbumPosts
                                    where posts.IdUsersAlbum == idAlbum
                                    select posts.IdPost).ToArrayAsync();
            Task<string[]> photoPath = (from page in context.Images
                                        join user in context.UsersImages on page.Id equals user.IdImage
                                        where user.IdUser == id && idPosts.Result.Contains(user.Id)
                                        select page.Path).ToArrayAsync();
            Task<string[]> namePhoto = (from page in context.UsersImages
                                        join user in context.Images on page.IdImage equals user.Id
                                        where page.IdUser == id
                                        select page.Name).ToArrayAsync();
            int i = 0;
            List<string> resultList = new List<string>();
            foreach (string str in photoPath.Result)
            {
                resultList.Add(str);
                resultList.Add(namePhoto.Result[i]);
                i++;
            }
            return resultList;
        }

        public static void AddNewPhoto(string nameImg, string path, int idAlbum, string idActiveUser)
        {
            TokenDatabaseContext context = new TokenDatabaseContext();
            Images image = new Images();
            image.Path = path;
            context.Images.Add(image);
            context.SaveChanges();
            Task<int> idPage = (from u in context.Images
                                where u.Path == path
                                select u.Id).FirstOrDefaultAsync();
            UsersImages users = new UsersImages();
            users.IdUser = idActiveUser;
            users.IdImage = idPage.Result;
            users.Name = nameImg;
            context.UsersImages.Add(users);
            context.SaveChanges();
            int idPost = (from posts in context.UsersImages
                          select posts.Id).ToList().Last();
            UserAlbumPosts userAlbumPosts = new UserAlbumPosts();
            userAlbumPosts.IdUsersAlbum = idAlbum;
            userAlbumPosts.IdPost = idPost;
            context.UserAlbumPosts.Add(userAlbumPosts);
            context.SaveChanges();
        }

        public static List<string> SearchPosts()
        {
            TokenDatabaseContext context = new TokenDatabaseContext();
            Task<string[]> authors = (from user in context.AspNetUsers
                                      join userPage in context.UsersImages on user.Id equals userPage.IdUser
                                      select user.UserName).ToArrayAsync();
            authors.Result.Reverse();
            Task<string[]> paths = (from imageImg in context.Images
                                    join imageUsImg in context.UsersImages on imageImg.Id equals imageUsImg.IdImage
                                    select imageImg.Path).ToArrayAsync();
            Task<int[]> idPosts = (from podtsId in context.UsersImages
                                   select podtsId.Id).ToArrayAsync();
            Task<string[]> idUser = (from podtsId in context.UsersImages
                                     select podtsId.IdUser).ToArrayAsync();
            Task<int?[]> idAlbum = (from albums in context.UserAlbumPosts
                                    join post in context.UsersImages on albums.IdPost equals post.Id
                                    select albums.IdUsersAlbum).ToArrayAsync();
            idUser.Result.Reverse();
            idPosts.Result.Reverse();
            paths.Result.Reverse();
            idAlbum.Result.Reverse();
            int index = 0;
            List<string> posts = new List<string>();
            foreach (string author in authors.Result)
            {
                posts.Add(idPosts.Result[index].ToString());
                posts.Add(author);
                posts.Add(paths.Result[index]);
                posts.Add(idUser.Result[index].ToString());
                posts.Add(idAlbum.Result[index].ToString());
                index++;
            }
            return posts;
        }

        public static List<string> SearchComments(int idPost)
        {
            TokenDatabaseContext context = new TokenDatabaseContext();
            Task<string[]> listUsers = (from userName in context.AspNetUsers
                                        join postId in context.UsersImagesComments on userName.Id equals postId.IdUser
                                        where idPost == postId.IdPost
                                        select userName.UserName).ToArrayAsync();
            Task<string[]> listComments = (from comment in context.UsersImagesComments
                                           where idPost == comment.IdPost
                                           select comment.Comment).ToArrayAsync();
            List<string> listCommentsUsers = new List<string>();
            int i = 0;
            foreach (string str in listUsers.Result)
            {
                listCommentsUsers.Add(str);
                listCommentsUsers.Add(listComments.Result[i]);
                i++;
            }
            return listCommentsUsers;
        }


        public static List<string> AuthorNameAndImagePath(int idPost)
        {
            TokenDatabaseContext context = new TokenDatabaseContext();
            Task<string> autorPost = (from author in context.AspNetUsers
                                      join post in context.UsersImages on author.Id equals post.IdUser
                                      where post.Id == idPost
                                      select author.UserName).FirstOrDefaultAsync();
            Task<string> authorId = (from author in context.AspNetUsers
                                     join post in context.UsersImages on author.Id equals post.IdUser
                                     where post.Id == idPost
                                     select author.Id).FirstOrDefaultAsync();
            Task<string> imagePath = (from path in context.Images
                                      join post in context.UsersImages on path.Id equals post.IdImage
                                      where post.Id == idPost
                                      select path.Path).FirstOrDefaultAsync();
            List<string> list = new List<string>();
            list.Add(autorPost.Result);
            list.Add(imagePath.Result);
            list.Add(authorId.ToString());
            return list;
        }

        public static void DeletePhoto(string nameOfPhoto, int idAlbum, string activeUserId)
        {
            TokenDatabaseContext context = new TokenDatabaseContext();
            Task<UsersImages> user = (from image in context.UsersImages
                                      join albums in context.UserAlbumPosts on image.Id equals albums.IdPost
                                      where image.Name == nameOfPhoto && image.IdUser == activeUserId && albums.IdUsersAlbum == idAlbum
                                      select image).FirstOrDefaultAsync();
            context.UsersImages.Attach(user.Result);
            context.UsersImages.Remove(user.Result);
            context.SaveChanges();
        }

        public static void AddComment(int idPost, string text)
        {
            TokenDatabaseContext context = new TokenDatabaseContext();
            //int activeUserId = WorkWithDb.getInformationActiveUser().Id; того чела который пришёл
            UsersImagesComments comment = new UsersImagesComments();
            comment.IdPost = idPost;
            //comment.IdUser = activeUserId;
            comment.Comment = text;
            context.UsersImagesComments.Add(comment);
            context.SaveChanges();
        }


        public static int AddAlbum(string idUser, string nameAlbum)
        {
            TokenDatabaseContext context = new TokenDatabaseContext();
            Album album = new Album();
            album.NameOfAlbum = nameAlbum;
            context.Album.Add(album);
            context.SaveChanges();
            int idAlbum = (from u in context.Album
                           select u.Id).ToList().Last();
            UserAlbum userAlbum = new UserAlbum();
            userAlbum.UserId = idUser;
            userAlbum.AlbumId = idAlbum;
            context.UserAlbum.Add(userAlbum);
            context.SaveChanges();
            return idAlbum;
        }

        public static List<string> SearchAlbum(string idUser)
        {
            TokenDatabaseContext context = new TokenDatabaseContext();
            Task<UserAlbum[]> userAlbum = (from u in context.UserAlbum
                                           where u.UserId == idUser
                                           select u).ToArrayAsync();
            Task<string[]> nameAlbum = (from albumName in context.Album
                                        join userAlbums in context.UserAlbum on albumName.Id equals userAlbums.AlbumId
                                        where userAlbums.UserId == idUser
                                        select albumName.NameOfAlbum).ToArrayAsync();
            var index = 0;
            List<string> listIdAlbumAndNameAlbum = new List<string>();
            foreach (UserAlbum album in userAlbum.Result)
            {
                listIdAlbumAndNameAlbum.Add(album.Id.ToString());
                listIdAlbumAndNameAlbum.Add(nameAlbum.Result[index]);
                index++;
            }
            return listIdAlbumAndNameAlbum;
        }
    }
}
