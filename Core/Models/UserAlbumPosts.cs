using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class UserAlbumPosts
    {
        public int Id { get; set; }
        public int? IdUsersAlbum { get; set; }
        public int? IdPost { get; set; }

        public UsersImages IdPostNavigation { get; set; }
        public UserAlbum IdUsersAlbumNavigation { get; set; }
    }
}
