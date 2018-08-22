using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class AspNetUsers
    {
        public AspNetUsers()
        {
            Table = new HashSet<Table>();
            UserAlbum = new HashSet<UserAlbum>();
            UsersImages = new HashSet<UsersImages>();
            UsersImagesComments = new HashSet<UsersImagesComments>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Surname { get; set; }
        public string Pseudonym { get; set; }
        public string FieldOfActivity { get; set; }
        public string City { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        public AspNetUserTokens AspNetUserTokens { get; set; }
        public ICollection<Table> Table { get; set; }
        public ICollection<UserAlbum> UserAlbum { get; set; }
        public ICollection<UsersImages> UsersImages { get; set; }
        public ICollection<UsersImagesComments> UsersImagesComments { get; set; }
    }
}
