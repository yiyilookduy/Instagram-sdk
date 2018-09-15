using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiyilook.Instagram.Sdk.Models
{
    public class UserInfo
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string ProfilePicUrl { get; set; }
        public bool isPrivate { get; set; }
        public bool isVerified { get; set; }
        public bool isBusiness { get; set; }
        public int Following { get; set; }
        public int Followers { get; set; }
        public int PostCount { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

    }
}
