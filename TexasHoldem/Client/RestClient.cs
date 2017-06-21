﻿using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;

namespace Client
{
    public class RestClient
    {
        //private static string _endPoint =  "http://132.73.197.182:3000/api/";
        private static string _endPoint = "http://localhost:57856/api/";
           
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
            var request = (HttpWebRequest)WebRequest.Create(_endPoint + controller);
            request.Method = "POST";
            WriteData(request, data);
            var ans = PerformRequest(request);
            return ans;  
        }

        public static string MakeGetRequest(string controller)
        {
            var request = (HttpWebRequest)WebRequest.Create(_endPoint + controller);
            request.Method = "GET";
            var ans = PerformRequest(request);
            return ans;
        }

        public static string MakePutRequest(string controller, string data)
        {
            string ans = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_endPoint + controller);
            request.Method = "PUT";
            WriteData(request, data);
            ans = PerformRequest(request);
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

        public static string GetEndPoint()
        {
            return _endPoint;
        }

    }
}