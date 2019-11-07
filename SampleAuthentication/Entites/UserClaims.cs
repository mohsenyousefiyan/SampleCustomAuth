using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAuthentication.Entites
{
    public class UserClaims
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
