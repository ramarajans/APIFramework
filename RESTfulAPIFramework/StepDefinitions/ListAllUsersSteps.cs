using RESTfulAPI.Libraries;
using System;
using System.Collections.Generic;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace RESTfulAPI
{
    [Binding]
    public class ListAllUsersSteps
    {
        private static readonly GenericUtils genUtils = new GenericUtils();
        private static readonly InputData inputObj = new InputData();

        Dictionary<string, string> output = new Dictionary<string, string>();

        string statusCode = string.Empty;
        string response = string.Empty;

        [Given(@"I have URI,URL and Content Type")]
        public void InitSteps()
        {
            inputObj.Method = "GET";
            inputObj.Url = "users";
            inputObj.PayLoad = "";

            Console.WriteLine();
            Console.WriteLine("method --> " + inputObj.Method + " , url --> " + inputObj.Url);
            Console.WriteLine();
        }

        [When(@"I perform GET method")]
        public void InvokeRESTMethod()
        {
            try
            {
                Console.WriteLine();
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

        [Then(@"I should get OK response with all the user listed")]
        public void ValidateResponse()
        {
            try
            {
                Console.WriteLine();

                if ("OK".Equals(statusCode))
                    Console.WriteLine(inputObj.Method + " method is successful");
                else
                    Console.WriteLine(inputObj.Method + " method is not successful");

                dynamic content = genUtils.ParseResponse(response);

                int totalUsers = content.total;

                Assert.IsTrue(totalUsers > 0);

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occurred : " + e.ToString());
            }
        }
    }
}
