using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiyilook.Instagram.Sdk.Models
{
    public class PostInfo
    {
        public PostInfo() {}
        public PostInfo(string id, string shortCode, string urlImagePost, string content)
        {
            Id = id;
            ShortCode = shortCode;
            UrlImagePost = urlImagePost;
            Content = content;
        }

        public string Id { get; set; }
        public string ShortCode { get; set; }
        public string UrlImagePost { get; set; }
        public string Content { get; set; }


    }
}
