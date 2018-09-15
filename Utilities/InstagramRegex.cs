using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Yiyilook.Instagram.Sdk.Models;
namespace Yiyilook.Instagram.Sdk.Utilities
{
    public class InstagramRegex
    {
        private static InstagramRegex instance;
        public static InstagramRegex Instance { get => instance?? (instance = new InstagramRegex()); private set => instance = value; }


        private IDictionary<string, Regex> dictionaryRegexes = new Dictionary<string, Regex>()
        {
            {"IdUser", new Regex(@"""profilePage_(.*?)"",""show_suggested_profiles"":") },
            {"Username", new Regex(@"username"":""(.*?)"",""full_name"":")  },
            {"EndCursor", new Regex(@"end_cursor"":""(.*?)""},""edges"":") },
            {"Csrf", new Regex(@"csrf_token"":""(.*?)"",""viewer""")},

            // PostInfo
            {"IdPost", new Regex(@"id"":""(.*?)"",""dimensions"":") },
            {"ShortCode", new Regex(@"shortcode"":""(.*?)"",""edge_media_preview_comment"":") },
            {"ImagePost", new Regex(@"thumbnail_src"":""(.*?)"",""thumbnail_resources"":") },
            {"Content", new Regex(@"{""text"":""(.*?)""}") },

            // UserInfo
            {"IdUserInfo", new Regex(@"pk"": (.*?), """) },
            {"UsernameInfo", new Regex(@"username"": ""(.*?)"", """) },
            {"ProfilePicUrl", new Regex(@"profile_pic_url"": ""(.*?)"", """) },
            {"isPrivate", new Regex(@"is_private"": (.*?), """) },
            {"isVerified", new Regex(@"is_verified"": (.*?), """) },
            {"isBusiness", new Regex(@"is_business"": (.*?), """) },
            {"Following", new Regex(@"following_count"": (.*?), """) },
            {"Followers", new Regex(@"follower_count"": (.*?), """) },
            {"PostCount", new Regex(@"media_count"": (.*?), """) },
            {"Email", new Regex(@"public_email"": ""(.*?)"", """) },
            {"Phone", new Regex(@"public_phone_number"": ""(.*?)"", """) }
        };

        public string MatchResult(string key, string content)
        {
            if (dictionaryRegexes.ContainsKey(key))
            {
                 return dictionaryRegexes[key].Match(content).Groups[1].ToString();
            }
            else return string.Empty;
        }

        public Match Match(string key, string content)
        {
            if (dictionaryRegexes.ContainsKey(key))
            {
                return dictionaryRegexes[key].Match(content);
            }
            else return System.Text.RegularExpressions.Match.Empty;
        }

        public MatchCollection Matches(string key, string content)
        {
            if (dictionaryRegexes.ContainsKey(key))
            {
                return dictionaryRegexes[key].Matches(content);
            }
            else return null;
        }

        /// <summary>
        /// Get Post info via regexes math content, and return a tuple of MatchCollection
        /// Item1 is a IdPost
        /// Item2 is a ShortCode
        /// Item3 is a Url of ImagePost
        /// Item4 is a Content of post
        /// </summary>
        /// <param name="content"> Json string from request of InstagramQuery</param>
        /// <returns></returns>
        public Tuple<MatchCollection, MatchCollection, MatchCollection, MatchCollection> GetPostsInfo(string content)
        {
            return new Tuple<MatchCollection, MatchCollection, MatchCollection, MatchCollection>
            (
                dictionaryRegexes["IdPost"].Matches(content),
                dictionaryRegexes["ShortCode"].Matches(content),
                dictionaryRegexes["ImagePost"].Matches(content),
                dictionaryRegexes["Content"].Matches(content)
            );
        }

        public PostInfo GetPostInfo(string content)
        {
            return new PostInfo()
            {
                Content = dictionaryRegexes["Content"].Match(content).Groups[1].ToString(),
                Id = dictionaryRegexes["IdPost"].Match(content).Groups[1].ToString(),
                ShortCode = dictionaryRegexes["ShortCode"].Match(content).Groups[1].ToString(),
                UrlImagePost = dictionaryRegexes["ImagePost"].Match(content).Groups[1].ToString()
            };
        }

        public UserInfo GetUserInfo(string content)
        {
            return new UserInfo()
            {
                Id = dictionaryRegexes["IdUserInfo"].Match(content).Groups[1].ToString(),
                Email = dictionaryRegexes["Email"].Match(content).Groups[1].ToString() != string.Empty ? dictionaryRegexes["Email"].Match(content).Groups[1].ToString(): "Not have Email",
                Followers = int.TryParse(dictionaryRegexes["Followers"].Match(content).Groups[1].ToString(), out int FlagFollowers) ? FlagFollowers : 0,
                Following = int.TryParse(dictionaryRegexes["Following"].Match(content).Groups[1].ToString(), out int FlagFollowing) ? FlagFollowing : 0,
                Phone = dictionaryRegexes["Phone"].Match(content).Groups[1].ToString() != string.Empty ? dictionaryRegexes["Phone"].Match(content).Groups[1].ToString() : "Not have phone",
                PostCount = int.TryParse(dictionaryRegexes["PostCount"].Match(content).Groups[1].ToString(), result: out int FlagPostCount) ? FlagPostCount : 0,
                ProfilePicUrl = dictionaryRegexes["ProfilePicUrl"].Match(content).Groups[1].ToString(),
                Username = dictionaryRegexes["UsernameInfo"].Match(content).Groups[1].ToString(),
                isBusiness = bool.TryParse(dictionaryRegexes["isBusiness"].Match(content).Groups[1].ToString(), out bool FlagIsBusiness) ? FlagIsBusiness : false,
                isPrivate = bool.TryParse(dictionaryRegexes["isPrivate"].Match(content).Groups[1].ToString(), out bool FlagisPrivate) ? FlagisPrivate : false,
                isVerified = bool.TryParse(dictionaryRegexes["isVerified"].Match(content).Groups[1].ToString(), out bool FlagisVerified) ? FlagisVerified : false
            };


        }
    }
}
