using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class UsersImages
    {
        public UsersImages()
        {
            UserAlbumPosts = new HashSet<UserAlbumPosts>();
            UsersImagesComments = new HashSet<UsersImagesComments>();
        }

        public int Id { get; set; }
        public string IdUser { get; set; }
        public int IdImage { get; set; }
        public string Name { get; set; }

        public Images IdImageNavigation { get; set; }
        public AspNetUsers IdUserNavigation { get; set; }
        public ICollection<UserAlbumPosts> UserAlbumPosts { get; set; }
        public ICollection<UsersImagesComments> UsersImagesComments { get; set; }
    }
}
