using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;

namespace MFAadapter.Helpers
{
    public class ApiCallerHelper
    {
        // This method will not 
        public bool GetHttpResult(bool useApi)
        {
            if (useApi)
            {
                var randomResponse = WebRequest.Create("https://dummy-true-false-api");

                var response = randomResponse.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        // We suppose that this will be true or false randomly according to task description.
                        string responseString = reader.ReadToEnd();
                        bool result = JsonConvert.DeserializeObject<bool>(responseString);
                        return result;
                    }
                }
            }

            return true;
        }
    }
}
