using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class UserAlbum
    {
        public UserAlbum()
        {
            UserAlbumPosts = new HashSet<UserAlbumPosts>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public int? AlbumId { get; set; }

        public Album Album { get; set; }
        public AspNetUsers User { get; set; }
        public ICollection<UserAlbumPosts> UserAlbumPosts { get; set; }
    }
}
