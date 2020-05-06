using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Configuration;
using System.Net.Http;
using System.Xml.XPath;
using Google.Protobuf.Collections;
using Newtonsoft.Json;
using RESTfulAPI.Libraries;

namespace RESTfulAPI
{    
    class GenericUtils
    {
        /// <summary>
        /// This method is to read the input jason file and parse it into json object
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public JObject ReadInputPayload(string fileName)
        {
            JObject jSONData = JObject.Parse(File.ReadAllText(ConfigurationSettings.AppSettings["InputPayloadsPath"] + fileName + ".json"));
            return jSONData;
        }

        /// <summary>
        /// This method is for converting the output response into a dynamic object
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public dynamic ParseResponse(string response)
        {
            dynamic content = Newtonsoft.Json.JsonConvert.DeserializeObject(response);

            return content;
        }

        /// <summary>
        /// This method is for performing HTTP methods.
        /// Method will accept an object as a parameter which will have details like method, base URI, URL, content type and input payload if appicable.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Dictionary<string,string> HTTPInvoke(InputData input)
        {
            string output = string.Empty;
            string endResult = string.Empty;
            Dictionary<string, string> outputDict = new Dictionary<string, string>();

            using (var client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri(input.baseURI);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(input.ContentType));

                if ("GET".Equals(input.Method))
                {
                    if (!input.PayLoad.Equals(string.Empty))
                        Console.WriteLine("Input payload is not required for GET method.Hence, not considering!!");

                    var response = client.GetAsync(input.Url);
                    response.Wait();
                    var result = response.Result;
                    Console.WriteLine("Result : "+result);
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    output = readTask.Result;
                    Console.WriteLine("Response Code  --->  " + result.StatusCode);
                    Console.WriteLine("Response  --->  " + output);

                    outputDict.Add("StatusCode", result.StatusCode.ToString());
                    outputDict.Add("Response", output);
                }
                else if ("POST".Equals(input.Method))
                {
                    var inputPayload = ReadInputPayload(input.PayLoad);
                    var response = client.PostAsJsonAsync(input.Url, inputPayload);

                    response.Wait();
                    var result = response.Result;
                    Console.WriteLine("Result : " + result);

                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                  
                    output = readTask.Result;

                    Console.WriteLine("Response Code  --->  " + result.StatusCode);
                    Console.WriteLine("Response  --->  " + output);

                    outputDict.Add("StatusCode", result.StatusCode.ToString());
                    outputDict.Add("Response", output);
                }
                else if ("PUT".Equals(input.Method))
                {
                    var inputPayload = ReadInputPayload(input.PayLoad);
                    var response = client.PutAsJsonAsync(input.Url, inputPayload);
                    response.Wait();

                    var result = response.Result;
                    Console.WriteLine("Result : " + result);
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    output = readTask.Result;

                    Console.WriteLine("Response Code  --->  " + result.StatusCode);
                    Console.WriteLine("Response  --->  " + output);

                    outputDict.Add("StatusCode", result.StatusCode.ToString());
                    outputDict.Add("Response", output);               
                }
                else if ("DELETE".Equals(input.Method))
                {
                    if (!input.PayLoad.Equals(string.Empty))
                        Console.WriteLine("Input payload is not required for DELETE method. Hence, not considering!!");

                    var response = client.DeleteAsync(input.Url);
                    response.Wait();

                    var result = response.Result;
                    Console.WriteLine("Result : " + result);
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    output = readTask.Result;

                    Console.WriteLine("Response Code  --->  " + result.StatusCode);
                    Console.WriteLine("Response  --->  " + output);

                    outputDict.Add("StatusCode", result.StatusCode.ToString());
                    outputDict.Add("Response", output);
                }   
            }
            return outputDict;
        }
    }
}
