using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;

namespace EOSCRM.Web.Common
{
    public class SpeedSMSAPI
    {       
        public const int TYPE_QC = 1;
        public const int TYPE_CSKH = 2;
        public const int TYPE_BRANDNAME = 3;
        const string rootURL = "http://api.speedsms.vn/index.php";
        private string accessToken = "Sda-p5D5QeSjNR767624U77PLnvWcbKY";

        public SpeedSMSAPI()
        {

        }

        public SpeedSMSAPI(string token)
        {
            this.accessToken = token;
        }

        public string getUserInfo()
        {
            string url = rootURL + "/user/info";
            NetworkCredential myCreds = new NetworkCredential(accessToken, ":x");
            WebClient client = new WebClient();
            client.Credentials = myCreds;
            Stream data = client.OpenRead(url);
            StreamReader reader = new StreamReader(data);
            return reader.ReadToEnd();
        }

        public string sendSMS(string phone, string content, int type, string brandname)
        {
            string url = rootURL + "/sms/send";
            if (phone.Length <= 0 || phone.Length < 10 || phone.Length > 11)
                return "";
            if (content.Equals(""))
                return "";
            if (type < TYPE_QC || type > TYPE_BRANDNAME)
                return "";
            if (type == TYPE_BRANDNAME && brandname.Equals(""))
                return "";
            if (!brandname.Equals("") && brandname.Length > 11)
                return "";

            NetworkCredential myCreds = new NetworkCredential(accessToken, ":x");
            WebClient client = new WebClient();
            client.Credentials = myCreds;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            string json = "{\"to\":[\"" + phone + "\"], \"content\": \"" + content + "\", \"type\":" + type + ", \"brandname\": \"" + brandname + "\"}";
            return client.UploadString(url, json);
        }
        
    }
}