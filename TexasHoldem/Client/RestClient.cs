using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;

namespace Client
{
    public class RestClient
    {
        const string AZURE_ADDRESS = "http://texasholdem2017.azurewebsites.net/api/";
       // private static string _endPoint = AZURE_ADDRESS;
        private static string _endPoint = "http://localhost:57856/api/";

        private static void WriteData(HttpWebRequest request, string data)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            request.ContentType = "application/json";
            request.ContentLength = buffer.Length;

            var reqStream = request.GetRequestStream();
            reqStream.Write(buffer, 0, buffer.Length);
            reqStream.Close();
        }   

        public static string MakePostRequest(string controller, string data)
        {
            string ans = "";
            SetController(controller);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_endPoint);
            request.Method = "POST";
            WriteData(request, data);
            ans = PerformRequest(request);
            _endPoint = "http://localhost:57856/api/";
            //    _endPoint = AZURE_ADDRESS;
            return ans;
        }

        public static string MakeGetRequest(string controller)
        {
            string ans = "";
            SetController(controller);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_endPoint);
            request.Method = "GET";
            ans = PerformRequest(request);
            _endPoint = "http://localhost:57856/api/";
       //     _endPoint = AZURE_ADDRESS;
            return ans;
        }

        private static string PerformRequest(WebRequest request)
        {
            string strResponseValue = "";
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException("Error code: " + response.StatusCode.ToString());
                }
                //Process the response stream... (JSON/XML/HTML...)
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                        }//End of Stream Reader
                    }
                }//End of using ResponseStream
            }
            catch (Exception e)
            {
                return "Failed to connect to remote server";
            }
            return strResponseValue;
        }

        private static void SetController(string suffix)
        {
            _endPoint = _endPoint + suffix;
        }

        public static string GetEndPoint()
        {
            return _endPoint;
        }

    }
}