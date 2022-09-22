using MFAadapter.Helpers;
using MFAadapter.Models;
using Microsoft.IdentityServer.Web.Authentication.External;
using System;
using System.Diagnostics;
using System.Net;
using System.Security.Authentication;
using Claim = System.Security.Claims.Claim;

namespace MFAadapter
{
    public class ADFSMFAAdapter : IAuthenticationAdapter
    {
        private ADUserHelper aDUserHelper;
        private ApiCallerHelper apiCallerHelper;
        private EventLog eventLog;
        private bool isAuthSuccess = false;

        public ADFSMFAAdapter()
        {
            aDUserHelper = new ADUserHelper();
            apiCallerHelper = new ApiCallerHelper();
        }

        public IAuthenticationAdapterMetadata Metadata
        {
            get { return new PluginMetadata(); }
        }

        public IAdapterPresentation BeginAuthentication(Claim identityClaim, HttpListenerRequest request, IAuthenticationContext authContext)
        {
            try
            {
                bool isUserActive = aDUserHelper.GetUserActiveInfo(authContext.Data["UserIdentifier"].ToString());

                // Step 1:
                // Use a mocked service to look up the status of this user from AD.
                // If the user is enabled, proceed to step 2, otherwise fail the authentication immediately.
                if (isUserActive)
                {
                    bool authResult;

                    // This is for testing purposes because there is no real API that returns the result in this example.
                    if (authContext.Data["UseApi"] != null)
                    {
                        bool useApi = Convert.ToBoolean(authContext.Data["UseApi"]);
                        authResult = apiCallerHelper.GetHttpResult(useApi);
                    }
                    else
                    {
                        authResult = apiCallerHelper.GetHttpResult(false);
                    }

                    // Step 2:
                    // Use a mocked service to invoke a dummy HTTP service, this service would randomly respond with true or false,
                    // if true, proceed to step 3, otherwise fail the authentication immediately.
                    if (authResult)
                    {
                        // Step 3:
                        // Log the event using Windows Event Log APIs. 
                        LogAuthEvent(authResult);
                        isAuthSuccess = true;
                    }
                    else
                    {
                        throw new AuthenticationException("User Authentication Failed");
                    }
                }
                else
                {
                    throw new AuthenticationException("User Authentication Failed");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return new PluginPresentationForm(isAuthSuccess);
        }

        public bool IsAvailableForUser(Claim identityClaim, IAuthenticationContext authContext)
        {
            return true; //its all available for now
        }

        public void OnAuthenticationPipelineLoad(IAuthenticationMethodConfigData configData) {}

        public void OnAuthenticationPipelineUnload() {}

        public IAdapterPresentation OnError(HttpListenerRequest request, ExternalAuthenticationException ex)
        {
            
            return new PluginPresentationForm(false);
        }

        public IAdapterPresentation TryEndAuthentication(IAuthenticationContext authContext, IProofData proofData, HttpListenerRequest request, out Claim[] outgoingClaims)
        {
            outgoingClaims = new Claim[0];

            // Step 4:
            // Mark the authentication as successful and let the ADFS handle the rest. 
            if (isAuthSuccess)
            {
                outgoingClaims = new[]
                {
                    // Return the required authentication method claim, indicating the particulate authentication method used.
                    new Claim( "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", "http://example.com/myauthenticationmethod1" )
                };
                return null;
            }
            else
            {
                return new PluginPresentationForm(false);
            }
        }

        private void LogAuthEvent(bool result)
        {
            using (eventLog = new EventLog("ADFS MFA Adapter"))
            {
                eventLog.Source = "ADFS MFA Adapter";
                eventLog.WriteEntry($"Web Api result: {result}", EventLogEntryType.Information);
            }
        }
    }
}