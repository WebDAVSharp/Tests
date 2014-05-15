using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using WebDAVSharp.Server.Exceptions;

namespace WebDAVTests
{
    /// <summary>
    /// Tests if the exceptions have the right statuscodes
    /// </summary>
    [TestClass]
    public class UnitTestExceptions
    {
        /// <summary>
        /// Checks if the <see cref="WebDavConflictException" /> has the statuscode 409 Conflict.
        /// </summary>
        [TestMethod]
        public void Exception_Conflict_Test()
        {
            var exception = new WebDavConflictException();
            Assert.AreEqual((int)HttpStatusCode.Conflict, exception.StatusCode);
        }

        /// <summary>
        /// Checks if the <see cref="WebDavForbiddenException" /> has the statuscode 403 Forbidden.
        /// </summary>
        [TestMethod]
        public void Exception_Forbidden_Test()
        {
            var exception = new WebDavForbiddenException();
            Assert.AreEqual((int)HttpStatusCode.Forbidden, exception.StatusCode);
        }

        /// <summary>
        /// Checks if the <see cref="WebDavInternalServerException" /> has the statuscode 500 Internal Server Error.
        /// </summary>
        [TestMethod]
        public void Exception_InternalServer_Test()
        {
            var exception = new WebDavInternalServerException();
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, exception.StatusCode);
        }

        /// <summary>
        /// Checks if the <see cref="WebDavLengthRequiredException" /> has the statuscode 411 Length Required.
        /// </summary>
        [TestMethod]
        public void Exception_LengthRequired_Test()
        {
            var exception = new WebDavLengthRequiredException();
            Assert.AreEqual((int)HttpStatusCode.LengthRequired, exception.StatusCode);
        }

        /// <summary>
        /// Checks if the <see cref="WebDavMethodNotAllowedException" /> has the statuscode 405 Method Not Allowed.
        /// </summary>
        [TestMethod]
        public void Exception_MethodNotAllowed_Test()
        {
            var exception = new WebDavMethodNotAllowedException();
            Assert.AreEqual((int)HttpStatusCode.MethodNotAllowed, exception.StatusCode);
        }

        /// <summary>
        /// Checks if the <see cref="WebDavNotFoundException" /> has the statuscode 404 Not Found.
        /// </summary>
        [TestMethod]
        public void Exception_NotFound_Test()
        {
            var exception = new WebDavNotFoundException();
            Assert.AreEqual((int)HttpStatusCode.NotFound, exception.StatusCode);
        }

        /// <summary>
        /// Checks if the <see cref="WebDavNotImplementedException" /> has the statuscode 501 Not Implemented.
        /// </summary>
        [TestMethod]
        public void Exception_NotImplemented_Test()
        {
            var exception = new WebDavNotImplementedException();
            Assert.AreEqual((int)HttpStatusCode.NotImplemented, exception.StatusCode);
        }

        /// <summary>
        /// Checks if the <see cref="WebDavPreconditionFailedException" /> has the statuscode 412 Precondition Failed.
        /// </summary>
        [TestMethod]
        public void Exception_PreconditionFailed_Test()
        {
            var exception = new WebDavPreconditionFailedException();
            Assert.AreEqual((int)HttpStatusCode.PreconditionFailed, exception.StatusCode);
        }

        /// <summary>
        /// Checks if the <see cref="WebDavUnauthorizedException" /> has the statuscode 401 Unauthorized.
        /// </summary>
        [TestMethod]
        public void Exception_Unauthorized_Test()
        {
            var exception = new WebDavUnauthorizedException();
            Assert.AreEqual((int)HttpStatusCode.Unauthorized, exception.StatusCode);
        }

        /// <summary>
        /// Checks if the <see cref="WebDavUnsupportedMediaTypeException" /> has the statuscode 415 Unsupported Media Type.
        /// </summary>
        [TestMethod]
        public void Exception_UnsupportedMediaType_Test()
        {
            var exception = new WebDavUnsupportedMediaTypeException();
            Assert.AreEqual((int)HttpStatusCode.UnsupportedMediaType, exception.StatusCode);
        }
    }
}
