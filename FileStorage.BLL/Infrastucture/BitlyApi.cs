using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace FileStorage.BLL.Infrastucture
{
    public static class BitlyApi
    {
        private const string HttpStatusOk = "200";
        private const string ApiSslUrl = "http://api-ssl.bitly.com/v3/shorten";
        private const string Login = "weynard";
        private const string ApiKey = "R_b8b5866a84cf413f8249020b61f7b646";

        public static string GetShortenedUrl(string longUrl)
        {
            string url = string.Format("{0}?format=xml&longUrl={1}&login={2}&apiKey={3}",
                ApiSslUrl, 
                HttpUtility.UrlEncode(longUrl), 
                Login,
                ApiKey);
            XDocument resultXml = XDocument.Load(url);
            if (resultXml.Descendants("status_code").FirstOrDefault().Value == HttpStatusOk)
            {
                XElement shortUrlElement = resultXml.Descendants("data").Elements("url").FirstOrDefault();
                if (shortUrlElement != null)
                {
                    return shortUrlElement.Value;
                }
            }

            return longUrl;
        }
    }
}
