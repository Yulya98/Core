using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class UsersImagesComments
    {
        public int Id { get; set; }
        public int IdPost { get; set; }
        public string IdUser { get; set; }
        public string Comment { get; set; }

        public UsersImages IdPostNavigation { get; set; }
        public AspNetUsers IdUserNavigation { get; set; }
    }
}
