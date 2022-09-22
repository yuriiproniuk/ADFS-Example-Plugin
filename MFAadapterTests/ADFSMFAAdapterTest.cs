using MFAadapter;
using MFAadapter.Models;
using Microsoft.IdentityServer.Web.Authentication.External;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Claims;

namespace MFAadapterTests
{
    [TestFixture]
    public class ADFSMFAAdapterTest
    {
        [Test]
        public void TestBeginAuthenticationSuccess()
        {
            // Arrange
            var authContext = new MyAuthContext();
            authContext.Data = new Dictionary<string, object>();
            authContext.Data.Add("UserIdentifier", "yuriiproniuk@softserveinc");
            authContext.Data.Add("UseApi", false);

            // Act
            ADFSMFAAdapter aDFSMFAAdapter = new ADFSMFAAdapter();
            var result = aDFSMFAAdapter.BeginAuthentication(null, null, authContext);

            var presentationResult = result as PluginPresentationForm;

            // Assert
            Assert.True(presentationResult.ApiAuthResult);
        }

        [Test]
        public void TestTryEndAuthenticationSuccess()
        {
            // Arrange
            var authContext = new MyAuthContext();
            Claim[] outgoingClaims;
            authContext.Data = new Dictionary<string, object>();
            authContext.Data.Add("UserIdentifier", "yuriiproniuk@softserveinc");
            authContext.Data.Add("UseApi", false);

            // Act
            ADFSMFAAdapter aDFSMFAAdapter = new ADFSMFAAdapter();
            var authResult = aDFSMFAAdapter.BeginAuthentication(null, null, authContext);

            var result = aDFSMFAAdapter.TryEndAuthentication(authContext, null, null, out outgoingClaims);

            // Assert
            Assert.IsNotEmpty(outgoingClaims);
            Assert.Null(result);
        }

        [Test]
        public void TestTryEndAuthenticationFailure()
        {
            // Arrange
            var authContext = new MyAuthContext();
            Claim[] outgoingClaims;
            authContext.Data = new Dictionary<string, object>();
            authContext.Data.Add("UserIdentifier", "yuriiproniuk@softserveinc");
            authContext.Data.Add("UseApi", false);

            // Act
            ADFSMFAAdapter aDFSMFAAdapter = new ADFSMFAAdapter();

            var result = aDFSMFAAdapter.TryEndAuthentication(authContext, null, null, out outgoingClaims);

            // Assert
            Assert.IsEmpty(outgoingClaims);
            Assert.NotNull(result);
            Assert.IsInstanceOf(typeof(PluginPresentationForm), result);
        }
    }

    // Implementing interface with custom class to pass parameters into methods for testing puproses
    public class MyAuthContext : IAuthenticationContext
    {
        public string ActivityId => throw new System.NotImplementedException();

        public string ContextId => throw new System.NotImplementedException();

        public int Lcid => throw new System.NotImplementedException();

        public Dictionary<string, object> Data { get; set; }
    }
}
