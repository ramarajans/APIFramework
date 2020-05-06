using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RESTfulAPI.Libraries;
using System;
using System.Collections.Generic;
using System.IO;
using TechTalk.SpecFlow;

namespace RESTfulAPI
{
    [Binding]
    public class RegisterUserSteps
    {
        private static readonly GenericUtils genUtils = new GenericUtils();
        private static readonly InputData inputObj = new InputData();

        Dictionary<string, string> output = new Dictionary<string, string>();

        string statusCode = string.Empty;
        string response = string.Empty;

        [Given(@"I have URI, URL , ContentType and '(.*)'")]
        public void InitSteps(string inputPayload)
        {
            inputObj.Method = "POST";
            inputObj.Url = "register";
            inputObj.PayLoad = inputPayload;

            Console.WriteLine();
            Console.WriteLine("method --> " + inputObj.Method + " , url --> " + inputObj.Url + " , payLoad --> " + inputObj.PayLoad);
            Console.WriteLine();
        }

        [When(@"I invoke POST method")]
        public void InvokeRESTMethod()
        {
            try
            {
                output = genUtils.HTTPInvoke(inputObj);
                output.TryGetValue("StatusCode", out statusCode);
                output.TryGetValue("Response", out response);
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occurred : " + e.ToString());
            }
        }

        [Then(@"I should get (.*) with response")]
        public void ValidateResponse(string resCode)
        {
            try
            {
                Console.WriteLine();

                if (resCode.Equals(statusCode))
                    Console.WriteLine(inputObj.Method + " method is successful");
                else
                    Console.WriteLine(inputObj.Method + " method is not successful");

                dynamic content = genUtils.ParseResponse(response);

                if ("Register_User".Equals(inputObj.PayLoad))
                {
                    int id = content.id;
                    string token = content.token;

                    Assert.IsTrue(id > 0 && token != null);
                }

                if ("Register_User_Unsuccessful".Equals(inputObj.PayLoad))
                {
                    string error = content.error;
                    Assert.AreEqual(error, "Missing password");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occurred : " + e.ToString());
            }

            Console.WriteLine();
        }
    }
}
