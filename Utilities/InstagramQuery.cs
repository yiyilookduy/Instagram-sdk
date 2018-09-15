using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiyilook.Instagram.Sdk.Utilities
{
    public class InstagramQuery
    {
        private static InstagramQuery instance;

        public const string BASE_URL = "https://www.instagram.com";

        public const string LOGIN_URL = "https://www.instagram.com/accounts/login/ajax/";

        public const string Account_INFO_BY_ID = "https://i.instagram.com/api/v1/users/{userId}/info/";

        public const string USER_FOLLOWER =
            @"https://www.instagram.com/graphql/query/?query_hash=56066f031e6239f35a904ac20c9f37d9&variables={""id"":""{IdPage}"",""first"":50,""after"":""{endcursor}""}";

        public const string POSTS_PAGE_USER =
            @"https://www.instagram.com/graphql/query/?query_hash=a5164aed103f24b03e7b7747a2d94e3c&variables={""id"":""{IdPage}"",""first"":50,""after"":""{endcursor}""}";

        public const string USER_INFO_JSON = @"https://i.instagram.com/api/v1/users/{IdUser}/info/";

        public static InstagramQuery Instance { get => instance?? (instance = new InstagramQuery()); private set => instance = value; }

        public string GetUserFollowerJson(string IdPage)
        {
            string result = USER_FOLLOWER;
            result = result.Replace("{IdPage}", IdPage);
            result = result.Replace("{endcursor}", "");
            return result;
        }

        public string GetUserInfoJson(string IdUser)
        {
            string result = USER_INFO_JSON;
            result = result.Replace("{IdUser}", IdUser);
            return result;
        }

        public string GetUserFollowerJson(string IdPage, string endcursor)
        {
            string result = USER_FOLLOWER;
            result = result.Replace("{IdPage}", IdPage);
            result = result.Replace("{endcursor}", endcursor);
            return result;
        }

        public string GetPostsOfPageOrUser(string IdPage)
        {
            string result = POSTS_PAGE_USER;
            result = result.Replace("{IdPage}", IdPage);
            result = result.Replace("{endcursor}", "");
            return result;
        }

        public string GetPostsOfPageOrUser(string IdPage, string endcursor)
        {
            string result = POSTS_PAGE_USER;
            result = result.Replace("{IdPage}", IdPage);
            result = result.Replace("{endcursor}", endcursor);
            return result;
        }
    }
}
