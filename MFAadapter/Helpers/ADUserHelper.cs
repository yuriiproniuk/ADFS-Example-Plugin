using System;

namespace MFAadapter.Helpers
{
    public class ADUserHelper
    {
        private JsonFileReader jsonFileReader;

        public ADUserHelper()
        {
            jsonFileReader = new JsonFileReader();
        }

        public bool GetUserActiveInfo(string upn)
        {
            try
            {
                var userStatus = jsonFileReader.GetUserStatus(upn);
                return userStatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
