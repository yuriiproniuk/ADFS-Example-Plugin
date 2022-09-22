using Microsoft.IdentityServer.Web.Authentication.External;

namespace MFAadapter.Models
{
    public class PluginPresentationForm : IAdapterPresentationForm
    {
        public bool ApiAuthResult { get; set; }

        public PluginPresentationForm(bool apiAuthResult)
        {
            ApiAuthResult = apiAuthResult;
        }

        public string GetFormHtml(int lcid)
        {
            string htmlTemplate = Resources.MfaForm; //todo we will implement this
            return htmlTemplate;
        }

        public string GetFormPreRenderHtml(int lcid)
        {
            return null;
        }

        
        public string GetPageTitle(int lcid)
        {
            return "ADFS MFA Adapter";
        }
    }
}
