using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTfulAPI.Libraries
{
   public class InputData
    {
            public string baseURI
            {
                get { return ConfigurationSettings.AppSettings["BaseURI"]; }
            }
            public string Method { get; set; }
            public string Url { get; set; }
            public string PayLoad { get; set; }
            public string ContentType
            {
                get { return ConfigurationSettings.AppSettings["ContentType"]; }
            }
            public override string ToString()
            {
                return string.Format(Method, Url, PayLoad);
            }
    }
}
