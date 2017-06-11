using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;

namespace Client
{
    public class RestClient
    {

        //const string AZURE_ADDRESS = "http://texasholdem2017.azurewebsites.net/api/";
        //const string AZURE_ADDRESS = "http://localhost:57856/api/";
        const string AZURE_ADDRESS = "http://192.168.1.4:80/api/";

        private static string _endPoint = AZURE_ADDRESS;

        private static void WriteData(WebRequest request, string data)
        {
            var buffer = Encoding.ASCII.GetBytes(data);
            request.ContentType = "application/json";
            request.ContentLength = buffer.Length;

            var reqStream = request.GetRequestStream();
            reqStream.Write(buffer, 0, buffer.Length);
            reqStream.Close();
        }   

        public static string MakePostRequest(string controller, string data)
        {
            SetController(controller);
            var request = (HttpWebRequest)WebRequest.Create(_endPoint);
            request.Method = "POST";
            WriteData(request, data);
            var ans = PerformRequest(request);
            _endPoint = AZURE_ADDRESS;
            return ans;  
        }

        public static string MakeGetRequest(string controller)
        {
            SetController(controller);
            var request = (HttpWebRequest)WebRequest.Create(_endPoint);
            request.Method = "GET";
            var ans = PerformRequest(request);
            _endPoint = AZURE_ADDRESS;
            return ans;
        }

        public static string MakePutRequest(string controller, string data)
        {
            string ans = "";
            SetController(controller);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_endPoint);
            request.Method = "PUT";
            WriteData(request, data);
            ans = PerformRequest(request);
            _endPoint = AZURE_ADDRESS;
            return ans;
        }   

        private static string PerformRequest(WebRequest request)
        {
            var strResponseValue = "";
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException("Error code: " + response.StatusCode);
                }
                //Process the response stream... (JSON/XML/HTML...)
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                        }//End of Stream Reader
                    }
                }//End of using ResponseStream
            }
            catch (Exception)
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