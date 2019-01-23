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
            string url =
                $"{ApiSslUrl}?format=xml&longUrl={HttpUtility.UrlEncode(longUrl)}&login={Login}&apiKey={ApiKey}";
            XDocument resultXml;
            try
            {
                resultXml = XDocument.Load(url);
            }
            catch
            {
                return longUrl;
            }

            var statusCode = resultXml.Descendants("status_code").FirstOrDefault();

            if (statusCode == null)
                return longUrl;

            if (statusCode.Value != HttpStatusOk)
                return longUrl;

            XElement shortUrlElement = resultXml.Descendants("data").Elements("url").FirstOrDefault();

            return shortUrlElement != null ? shortUrlElement.Value : longUrl;
        }
    }
}
