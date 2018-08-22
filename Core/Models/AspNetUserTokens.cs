using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class AspNetUserTokens
    {
        public string UserId { get; set; }
        public string Value { get; set; }

        public AspNetUsers User { get; set; }
    }
}
