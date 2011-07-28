using System;
using System.Net;
using Fraudpointer.API.Clients;
using Fraudpointer.API.Models;
using Fraudpointer.API.RequestWrappers;
using Fraudpointer.API.ResponseWrappers;
using Moq;
using NUnit.Framework;

namespace Fraudpointer.API.Tests.Unit.Clients
{
    /// <summary>
    /// This tests the mapping urls
    /// </summary>
    [TestFixture]
    public class ClientTests
    {
        [Test]
        public void CreateAssessmentSession_CorrectUrl_Ok()
        {
            // Fake return
            var ras = new ResponseAssessmentSession()
                          {
                              AssessmentSession = new AssessmentSession()
                          };
            var mockHttp = new Mock<IHttpWrapper>();
            mockHttp.Setup(x => x.Post<ResponseAssessmentSession>(It.IsAny<string>(), It.IsAny<RequestKey>()))
                .Returns(ras);

            // Client
            new Client(mockHttp.Object, "MyApiKey").CreateAssessmentSession();

            mockHttp.Verify(x => x.Post<ResponseAssessmentSession>("/assessment_sessions", It.IsAny<RequestKey>()), Times.Once());
        }

        /// <summary>
        /// Let's test error handling
        /// </summary>
        [Test]
        public void CreateAssessmentSession_SomethingHappens_ThrowClientException()
        {
            var mockHttp = new Mock<IHttpWrapper>();
            mockHttp.Setup(x => x.Post<ResponseAssessmentSession>(It.IsAny<string>(), It.IsAny<RequestKey>()))
                .Throws(new Exception());
            // Client
            Assert.Throws(typeof (ClientException)
                          , () => new Client(mockHttp.Object, "MyApiKey").CreateAssessmentSession());


        }

        /// <summary>
        /// Now web error handling
        /// </summary>
        [Test]
        public void CreateAssessmentSession_HttpError_ThrowClientException()
        {
            var mockHttp = new Mock<IHttpWrapper>();
            mockHttp.Setup(x => x.Post<ResponseAssessmentSession>(It.IsAny<string>(), It.IsAny<RequestKey>()))
                .Throws(new WebException("MyMessage"));
            // Client
            Assert.Throws(typeof(ClientException)
                          , () => new Client(mockHttp.Object, "MyApiKey").CreateAssessmentSession()
                          ,"MyMessage");


        }
    }
}
