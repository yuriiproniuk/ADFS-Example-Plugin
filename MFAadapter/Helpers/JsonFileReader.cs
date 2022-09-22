using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MFAadapter.Helpers
{
    public class JsonFileReader
    {
        public bool GetUserStatus(string upn)
        {
            string jsonStr = Encoding.UTF8.GetString(Resources.UserAccounts);
            List<Dictionary<string, string>> users = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonStr);
            var userStatus = users.Where(u => u["UserPrincipalName"] == upn).Select(u => u["IsActive"]).FirstOrDefault();
            bool result;
            bool.TryParse(userStatus, out result);
            return result;
        }
    }
}
