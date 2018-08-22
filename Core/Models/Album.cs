using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class Album
    {
        public Album()
        {
            Table = new HashSet<Table>();
            UserAlbum = new HashSet<UserAlbum>();
        }

        public int Id { get; set; }
        public string NameOfAlbum { get; set; }

        public ICollection<Table> Table { get; set; }
        public ICollection<UserAlbum> UserAlbum { get; set; }
    }
}
