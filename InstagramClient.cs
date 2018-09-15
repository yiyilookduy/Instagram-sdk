using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Web.Script.Serialization;

using Yiyilook.Instagram.Sdk.Exceptions;
using Yiyilook.Instagram.Sdk.Models;
using Yiyilook.Instagram.Sdk.Utilities;



namespace Yiyilook.Instagram.Sdk
{
    public class InstagramClient : IDisposable
    {

        public string Username;
        public string Password;

        private HttpHandler _http;

        private const int MaximumResultForRequest = 49;
        private const int TimeOutForRequest = 5000;


        public InstagramClient(string username, string password)
        {
            this.Password = password;
            this.Username = username;

            _http = new HttpHandler();


            Authorize();
        }

        void Authorize()
        {
            string htmlContent = _http.DownloadContent(InstagramQuery.BASE_URL);

            string CSRF = InstagramRegex.Instance.MatchResult("Csrf", htmlContent);

            _http.CSRF = CSRF;

            string creadential = $@"username={Username}&password={Password}";

            using (HttpWebResponse response = _http.SendPostRequest(InstagramQuery.LOGIN_URL, creadential))
            {
                if (response.Cookies["ds_user_id"] == null)
                    throw new UnAuthorizedException(InstagramClientErrors.CookieKeyUserNotFound);
            }
        }
        /// <summary>
        /// Yield return info of instagram post, return null if dont have result 
        /// </summary>
        /// <param name="IdPage"> ID of instagram user or page</param>
        /// <param name="numPost"> Num post want to get </param>
        /// <returns></returns>
        public IEnumerable<PostInfo> GetPostInfo(string UrlPage, int numPost)
        {
            string IdPage = GetIdUserInstagram(UrlPage);

            // Get request max number of post is 50 ( mean 0-49 ) 
            string UrlGetRequest = InstagramQuery.Instance.GetPostsOfPageOrUser(IdPage);
                
            string htmlContent = _http.DownloadContent(UrlGetRequest);


            var PostInfoGroup = InstagramRegex.Instance.GetPostsInfo(htmlContent);

            for (int i = 0; i < numPost; i++)
            {
                string ID = PostInfoGroup.Item1[i].Groups[1].ToString();
                string shortCode = PostInfoGroup.Item2[i].Groups[1].ToString();

                // Repare for content of post info
                JavaScriptSerializer jss = new JavaScriptSerializer();
                var content = jss.Deserialize<dynamic>(PostInfoGroup.Item3[i].Groups[0].ToString());
                string contentString = content["text"].ToString();
                contentString = contentString.Replace("\n", "\r\n");
               
                string urlImagePost = PostInfoGroup.Item4[i].Groups[1].ToString();
                PostInfo post = new PostInfo(ID, shortCode, urlImagePost, contentString);
                if (post == null) yield return null;
                else
                yield return post;
                
                // 49 is index max the request can create
                if (i == MaximumResultForRequest)
                {
                    // Get end_cursor for load more info for the next post

                    string end_cursor = InstagramRegex.Instance.MatchResult("EndCursor", htmlContent);

                    UrlGetRequest = InstagramQuery.Instance.GetPostsOfPageOrUser(IdPage, end_cursor);

                    htmlContent = _http.DownloadContent(UrlGetRequest);

                    PostInfoGroup = InstagramRegex.Instance.GetPostsInfo(htmlContent);

                    //
                    i = 0;
                }

            }
        }

        /// <summary>
        /// Get ID User page by URL PAGE or Name Alias
        /// </summary>
        /// <param name="UrlPage"></param>
        /// <returns></returns>
        public string GetIdUserInstagram(string UrlPage)
        {
            string htmlDocument = _http.DownloadContent(UrlPage);

            return InstagramRegex.Instance.MatchResult("IdUser", htmlDocument);
        }

        public IEnumerable<string> GetUsernameFollowers(string UrlPage)
        {
            string IdPage = GetIdUserInstagram(UrlPage);

            string UrlGet = InstagramQuery.Instance.GetUserFollowerJson(IdPage);


            string htmlDocument = _http.DownloadContent(UrlGet);

            var MatchesGroup = InstagramRegex.Instance.Matches("Username", htmlDocument);

            for (int i = 0; i < MatchesGroup.Count; i++)
            {
                yield return MatchesGroup[i].Groups[1].ToString();
                if (i == MatchesGroup.Count - 1)
                {
                    string end_cursor = InstagramRegex.Instance.MatchResult("EndCursor", htmlDocument);
                    if (end_cursor != "null")
                    {
                        i = 0;
                        UrlGet = InstagramQuery.Instance.GetUserFollowerJson(IdPage, end_cursor);

                        // Wait to load, the request has limit
                        // TODO optimize timeout to wait
                        Thread.Sleep(TimeOutForRequest);

                        htmlDocument = _http.DownloadContent(UrlGet);
                        MatchesGroup = InstagramRegex.Instance.Matches("Username", htmlDocument);
                    }
                    else break;
                }
            }

            yield return null;
        }


        public string GetUsername(string UrlPage)
        {
            string IdPage = GetIdUserInstagram(UrlPage);

            string htmlDocument = _http.DownloadContent(InstagramQuery.Instance.GetUserInfoJson(IdPage));

            return InstagramRegex.Instance.MatchResult("IdUserInfo", htmlDocument);
        }

        public UserInfo GetUserInfo(string UrlPage)
        {
            string IdPage = GetIdUserInstagram(UrlPage);

            string htmlDocument = _http.DownloadContent(InstagramQuery.Instance.GetUserInfoJson(IdPage));

            return InstagramRegex.Instance.GetUserInfo(htmlDocument);
        }



        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

}
