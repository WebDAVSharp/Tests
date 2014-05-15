using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Net;
using WebDAVSharp.Server;
using WebDAVSharp.Server.Utilities;

namespace WebDAVTests
{
    /// <summary>
    /// Tests for the methods
    /// </summary>
    [TestClass]
    public class UnitTestMethods
    {
        private WebDavServer _server;

        /// <summary>
        /// The first method to be executed.
        /// This will start the WebDAV _server.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _server = WebDav.StartWebDavServer();
        }

        /// <summary>
        /// Try to start the WebDAV _server when it's already started.
        /// Expect an InvalidOperationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Server_IsStarted()
        {
            _server.Start();
        }

        #region PROPFIND

        /// <summary>
        /// Try to make a WebDAV Propfind request.
        /// Expect the statuscode 207 Multi-Status (WebDAV; RFC 4918).
        /// </summary>
        [TestMethod]
        public void Propfind_ValidRequest_TestMethod()
        {
            // get response
            var response = WebDavMethod.Propfind(WebDavConfig.WebDavTestBaseUri.ToString());
            var httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)WebDavStatusCode.MultiStatus, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV Propfind request.
        /// Expect the statuscode 207 Multi-Status (WebDAV; RFC 4918).
        /// </summary>
        [TestMethod]
        public void Propfind_ValidRequest_Depth0_TestMethod()
        {
            // get response
            var response = WebDavMethod.Propfind(WebDavConfig.WebDavTestBaseUri.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)WebDavStatusCode.MultiStatus, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV Propfind request.
        /// Expect the statuscode 207 Multi-Status (WebDAV; RFC 4918).
        /// </summary>
        [TestMethod]
        public void Propfind_ValidRequest_Depth1_TestMethod()
        {
            // get response
            var response = WebDavMethod.Propfind(WebDavConfig.WebDavTestBaseUri.ToString(), "1");
            var httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)WebDavStatusCode.MultiStatus, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV Propfind request.
        /// Expect the statuscode 207 Multi-Status (WebDAV; RFC 4918).
        /// </summary>
        [TestMethod]
        public void Propfind_ValidRequest_DepthInfinity_TestMethod()
        {
            // get response
            var response = WebDavMethod.Propfind(WebDavConfig.WebDavTestBaseUri.ToString(), "Infinity");
            var httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)WebDavStatusCode.MultiStatus, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV Propfind request to something non-existent.
        /// Expect the statuscode 404 Not Found.
        /// </summary>
        [TestMethod]
        public void Propfind_UnvalidRequest_TestMethod()
        {
            // create uri to non-existent file or folder
            var uri = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());

            // get response
            var response = WebDavMethod.Propfind(uri.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.NotFound, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV Propfind request.
        /// Expect the statuscode 207 Multi-Status (WebDAV; RFC 4918).
        /// </summary>
        //[TestMethod]
        public void Propfind_UnvalidRequest_UnAuthorized_TestMethod()
        {
            // get response
            var response = WebDavMethod.Propfind(WebDavConfig.WebDavTestBaseUri.ToString());
            var httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.Unauthorized, (int)httpWebResponse.StatusCode);
        }

        #endregion

        #region MKCOL

        /// <summary>
        /// Try to make a WebDAV MkCol request.
        /// Expect the statuscode 201 Created.
        /// </summary>
        [TestMethod]
        public void MkCol_ValidRequest_TestMethod()
        {
            // create uri to non-existent file or folder
            var uri = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)WebDavStatusCode.MultiStatus)
            {
                WebDavMethod.Delete(uri.ToString());
            }

            // get the response for creating the folder
            response = WebDavMethod.MkCol(uri.ToString());
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.Created, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV MkCol request.
        /// Expect the statuscode 405 Method Not Allowed.
        /// </summary>
        [TestMethod]
        public void MkCol_UnvalidRequest_CollectionAlreadyExists_TestMethod()
        {
            // create uri to non-existent file or folder
            var uri = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)WebDavStatusCode.MultiStatus)
            {
                WebDavMethod.Delete(uri.ToString());
            }

            // get the response for creating the folder
            WebDavMethod.MkCol(uri.ToString());
            response = WebDavMethod.MkCol(uri.ToString());
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.MethodNotAllowed, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV MkCol request.
        /// Expect the statuscode 409 Conflict.
        /// </summary>
        [TestMethod]
        public void MkCol_UnvalidRequest_IntermdiateCollectionsDontExist_TestMethod()
        {
            // create uri
            string path = Path.GetRandomFileName() + "/" + Path.GetRandomFileName();
            var uri = new Uri(WebDavConfig.WebDavTestBaseUri, path);

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)WebDavStatusCode.MultiStatus)
            {
                WebDavMethod.Delete(uri.ToString());
            }

            // get the response for creating the folder
            response = WebDavMethod.MkCol(uri.ToString());
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.Conflict, (int)httpWebResponse.StatusCode);
        }

        #endregion

        #region DELETE

        /// <summary>
        /// Try to make a WebDAV Delete request.
        /// Expect the statuscode 200 OK.
        /// </summary>
        [TestMethod]
        public void Delete_ValidRequest_TestMethod()
        {
            // create uri to file or folder
            var uri = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, create it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode != (int)WebDavStatusCode.MultiStatus)
            {
                WebDavMethod.MkCol(uri.ToString());
            }

            // get the response for deleting the folder
            response = WebDavMethod.Delete(uri.ToString());
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.OK, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV Delete request.
        /// Expect the statuscode 404 Not Found.
        /// </summary>
        [TestMethod]
        public void Delete_UnvalidRequest_NotFound_TestMethod()
        {
            // create uri to file or folder
            var uri = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, create it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)WebDavStatusCode.MultiStatus)
            {
                WebDavMethod.Delete(uri.ToString());
            }

            // get the response for deleting the folder
            response = WebDavMethod.Delete(uri.ToString());
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.NotFound, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV Delete request.
        /// Expect the statuscode 409 Conflict.
        /// </summary>
        [TestMethod]
        public void Delete_UnvalidRequest_Conflict_TestMethod()
        {
            // create uri to file or folder
            string path = Path.GetRandomFileName() + "/" + Path.GetRandomFileName();
            var uri = new Uri(WebDavConfig.WebDavTestBaseUri, path);

            // get the response for deleting the folder
            var response = WebDavMethod.Delete(uri.ToString());
            var httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.Conflict, (int)httpWebResponse.StatusCode);
        }

        #endregion

        #region PUT

        /// <summary>
        /// Try to make a WebDAV Put request.
        /// Expect the statuscode 201 Created.
        /// </summary>
        [TestMethod]
        public void Put_ValidRequest_TestMethod()
        {
            // create uri to non-existent file or folder
            var uri = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)WebDavStatusCode.MultiStatus)
            {
                WebDavMethod.Delete(uri.ToString());
            }

            // get the response for creating the folder
            response = WebDavMethod.Put(uri.ToString());
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.Created, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV Put request.
        /// Expect the statuscode 409 Conflict.
        /// </summary>
        [TestMethod]
        public void Put_UnvalidRequest_IntermdiateCollectionsDontExist_TestMethod()
        {
            // create uri
            string path = Path.GetRandomFileName() + "/" + Path.GetRandomFileName();
            var uri = new Uri(WebDavConfig.WebDavTestBaseUri, path);

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)WebDavStatusCode.MultiStatus)
            {
                WebDavMethod.Delete(uri.ToString());
            }

            // get the response for creating the folder
            response = WebDavMethod.Put(uri.ToString());
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.Conflict, (int)httpWebResponse.StatusCode);
        }

        #endregion

        #region COPY

        /// <summary>
        /// Try to make a WebDAV Copy request.
        /// Expect the statuscode 201 Created.
        /// </summary>
        [TestMethod]
        public void Copy_ValidRequest_TestMethod()
        {
            // create uri
            var uri1 = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());
            var uri2 = uri1 + "-Copy";

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri1.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)HttpStatusCode.NotFound)
            {
                WebDavMethod.Put(uri1.ToString());
            }

            // get the response for creating the folder
            response = WebDavMethod.Copy(uri1.ToString(), uri2);
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.Created, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV Copy request.
        /// Expect the statuscode 204 No Content.
        /// </summary>
        [TestMethod]
        public void Copy_ValidRequest_OverwriteTrue_TestMethod()
        {
            // create uri
            var uri1 = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());
            var uri2 = uri1 + "-Copy";

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri1.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)HttpStatusCode.NotFound)
            {
                WebDavMethod.Put(uri1.ToString());
            }

            // search for the file or folder
            response = WebDavMethod.Propfind(uri2, "0");
            httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)HttpStatusCode.NotFound)
            {
                WebDavMethod.Put(uri2);
            }

            // get the response for creating the folder
            response = WebDavMethod.Copy(uri1.ToString(), uri2, true);
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.NoContent, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV Copy request.
        /// Expect the statuscode 403 Forbidden.
        /// </summary>
        [TestMethod]
        public void Copy_UnvalidRequest_SameSourceAndDestination_TestMethod()
        {
            // create uri
            var uri1 = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri1.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)HttpStatusCode.NotFound)
            {
                WebDavMethod.Put(uri1.ToString());
            }

            // get the response for creating the folder
            response = WebDavMethod.Copy(uri1.ToString(), uri1.ToString());
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.Forbidden, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV Copy request.
        /// Expect the statuscode 409 Conflict.
        /// </summary>
        [TestMethod]
        public void Copy_UnvalidRequest_Conflict_TestMethod()
        {
            // create uri
            var uri1 = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());
            string path = Path.GetRandomFileName() + "/" + Path.GetRandomFileName();
            var uri2 = new Uri(WebDavConfig.WebDavTestBaseUri, path);

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri1.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)HttpStatusCode.NotFound)
            {
                WebDavMethod.Put(uri1.ToString());
            }

            // get the response for creating the folder
            response = WebDavMethod.Copy(uri1.ToString(), uri2.ToString());
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.Conflict, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV Copy request.
        /// Expect the statuscode 412 Precondition Failed.
        /// </summary>
        [TestMethod]
        public void Copy_UnvalidRequest_OverwriteFalse_TestMethod()
        {
            // create uri
            var uri1 = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());
            var uri2 = uri1 + "-Copy";

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri1.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)HttpStatusCode.NotFound)
            {
                WebDavMethod.Put(uri1.ToString());
            }

            // search for the file or folder
            response = WebDavMethod.Propfind(uri2, "0");
            httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)HttpStatusCode.NotFound)
            {
                WebDavMethod.Put(uri2);
            }

            // get the response for creating the folder
            response = WebDavMethod.Copy(uri1.ToString(), uri2);
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.PreconditionFailed, (int)httpWebResponse.StatusCode);
        }

        #endregion

        #region MOVE

        /// <summary>
        /// Try to make a WebDAV Move request.
        /// Expect the statuscode 201 Created.
        /// </summary>
        [TestMethod]
        public void Move_ValidRequest_TestMethod()
        {
            // create uri
            var uri1 = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());
            var uri2 = uri1 + "-Moved";

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri1.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)HttpStatusCode.NotFound)
            {
                WebDavMethod.Put(uri1.ToString());
            }

            // get the response for creating the folder
            response = WebDavMethod.Move(uri1.ToString(), uri2);
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.Created, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV Move request.
        /// Expect the statuscode 204 No Content.
        /// </summary>
        [TestMethod]
        public void Move_ValidRequest_OverwriteTrue_TestMethod()
        {
            // create uri
            var uri1 = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());
            var uri2 = uri1 + "-Moved";

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri1.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)HttpStatusCode.NotFound)
            {
                WebDavMethod.Put(uri1.ToString());
            }

            // search for the file or folder
            response = WebDavMethod.Propfind(uri2, "0");
            httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)HttpStatusCode.NotFound)
            {
                WebDavMethod.Put(uri2);
            }

            // get the response for creating the folder
            response = WebDavMethod.Move(uri1.ToString(), uri2, true);
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.NoContent, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV Move request.
        /// Expect the statuscode 403 Forbidden.
        /// </summary>
        [TestMethod]
        public void Move_UnvalidRequest_SameSourceAndDestination_TestMethod()
        {
            // create uri
            var uri1 = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri1.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)HttpStatusCode.NotFound)
            {
                WebDavMethod.Put(uri1.ToString());
            }

            // get the response for creating the folder
            response = WebDavMethod.Move(uri1.ToString(), uri1.ToString());
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.Forbidden, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV Move request.
        /// Expect the statuscode 409 Conflict.
        /// </summary>
        [TestMethod]
        public void Move_UnvalidRequest_Conflict_TestMethod()
        {
            // create uri
            var uri1 = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());
            string path = Path.GetRandomFileName() + "/" + Path.GetRandomFileName();
            var uri2 = new Uri(WebDavConfig.WebDavTestBaseUri, path);

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri1.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)HttpStatusCode.NotFound)
            {
                WebDavMethod.Put(uri1.ToString());
            }

            // get the response for creating the folder
            response = WebDavMethod.Move(uri1.ToString(), uri2.ToString());
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.Conflict, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV Move request.
        /// Expect the statuscode 412 Precondition Failed.
        /// </summary>
        [TestMethod]
        public void Move_UnvalidRequest_OverwriteFalse_TestMethod()
        {
            // create uri
            var uri1 = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());
            var uri2 = uri1 + "-Moved";

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri1.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)HttpStatusCode.NotFound)
            {
                WebDavMethod.Put(uri1.ToString());
            }

            // search for the file or folder
            response = WebDavMethod.Propfind(uri2, "0");
            httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)HttpStatusCode.NotFound)
            {
                WebDavMethod.Put(uri2);
            }

            // get the response for creating the folder
            response = WebDavMethod.Move(uri1.ToString(), uri2);
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.PreconditionFailed, (int)httpWebResponse.StatusCode);
        }
        #endregion

        #region LOCK

        /// <summary>
        /// Try to make a WebDAV Lock request.
        /// Expect the statuscode 200 Ok.
        /// </summary>
        [TestMethod]
        public void Lock_ValidRequest_TestMethod()
        {
            // create uri
            var uri = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)HttpStatusCode.NotFound)
            {
                WebDavMethod.Put(uri.ToString());
            }

            // get the response for creating the folder
            const string xmlBody = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                                   "<d:lockinfo xmlns:d=\"DAV:\"><d:lockscope><d:exclusive/>" +
                                   "</d:lockscope><d:locktype><d:write/></d:locktype><d:owner>" +
                                   "<d:href>http://www.contoso.com/~user/contact.htm</d:href>" +
                                   "</d:owner></d:lockinfo>";
            response = WebDavMethod.Lock(uri.ToString(), xmlBody);
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.OK, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV Lock request.
        /// Expect the statuscode 412 Precondition Failed.
        /// </summary>
        [TestMethod]
        public void Lock_UnvalidRequest_NoBody_TestMethod()
        {
            // create uri
            var uri = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)HttpStatusCode.NotFound)
            {
                WebDavMethod.Put(uri.ToString());
            }

            // get the response for creating the folder
            response = WebDavMethod.Lock(uri.ToString());
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.PreconditionFailed, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV Lock request.
        /// Expect the statuscode 201 Created.
        /// </summary>
        [TestMethod]
        public void Lock_ValidRequest_Created_TestMethod()
        {
            // create uri
            var uri = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)WebDavStatusCode.MultiStatus)
            {
                WebDavMethod.Delete(uri.ToString());
            }

            // get the response for creating the folder
            const string xmlBody = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><d:lockinfo xmlns:d=\"DAV:\"><d:lockscope><d:exclusive/></d:lockscope><d:locktype><d:write/></d:locktype><d:owner><d:href>http://www.contoso.com/~user/contact.htm</d:href></d:owner></d:lockinfo>";
            response = WebDavMethod.Lock(uri.ToString(), xmlBody);
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.Created, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV Lock request.
        /// Expect the statuscode 409 Conflict.
        /// </summary>
        [TestMethod]
        public void Lock_UnvalidRequest_Conflict_TestMethod()
        {
            // create uri
            var path = Path.GetRandomFileName() + "/" + Path.GetRandomFileName();
            var uri = new Uri(WebDavConfig.WebDavTestBaseUri, path);

            // get the response for creating the folder
            const string xmlBody = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                                   "<d:lockinfo xmlns:d=\"DAV:\"><d:lockscope>" +
                                   "<d:exclusive/></d:lockscope><d:locktype><d:write/>" +
                                   "</d:locktype><d:owner><d:href>http://www.contoso.com/~user/contact.htm" +
                                   "</d:href></d:owner></d:lockinfo>";
            var response = WebDavMethod.Lock(uri.ToString(), xmlBody);
            var httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.Conflict, (int)httpWebResponse.StatusCode);
        }
        #endregion

        #region OPTIONS

        /// <summary>
        /// Try to make a WebDAV Options request.
        /// Expect the statuscode 200 Ok.
        /// </summary>
        [TestMethod]
        public void Options_ValidRequest_TestMethod()
        {
            // get response
            var response = WebDavMethod.Options(WebDavConfig.WebDavTestBaseUri.ToString());
            var httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.OK, (int)httpWebResponse.StatusCode);
        }
        #endregion

        #region HEAD

        /// <summary>
        /// Try to make a WebDAV Head request.
        /// Expect the statuscode 200 Ok.
        /// </summary>
        [TestMethod]
        public void Head_ValidRequest_TestMethod()
        {
            // get response
            var response = WebDavMethod.Head(WebDavConfig.WebDavTestBaseUri.ToString());
            var httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.OK, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV Head request.
        /// Expect the statuscode 409 Conflict.
        /// </summary>
        [TestMethod]
        public void Head_UnvalidRequest_Conflict_TestMethod()
        {
            // create uri
            var path = Path.GetRandomFileName() + "/" + Path.GetRandomFileName();
            var uri = new Uri(WebDavConfig.WebDavTestBaseUri, path);

            // get response
            var response = WebDavMethod.Head(uri.ToString());
            var httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.Conflict, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV Head request.
        /// Expect the statuscode 404 Not Found.
        /// </summary>
        [TestMethod]
        public void Head_UnvalidRequest_NotFound_TestMethod()
        {
            // create uri
            var uri = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());

            // get response
            var response = WebDavMethod.Head(uri.ToString());
            var httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.NotFound, (int)httpWebResponse.StatusCode);
        }
        #endregion

        #region UNLOCK

        /// <summary>
        /// Try to make a WebDAV Unlock request.
        /// Expect the statuscode 204 No Content.
        /// </summary>
        [TestMethod]
        public void Unlock_ValidRequest_TestMethod()
        {
            // create uri
            var uri = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)HttpStatusCode.NotFound)
            {
                WebDavMethod.Put(uri.ToString());
            }

            // get the response for creating the folder
            const string xmlBody = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><d:lockinfo xmlns:d=\"DAV:\"><d:lockscope><d:exclusive/></d:lockscope><d:locktype><d:write/></d:locktype><d:owner><d:href>http://www.contoso.com/~user/contact.htm</d:href></d:owner></d:lockinfo>";
            WebDavMethod.Lock(uri.ToString(), xmlBody);

            // get the response for creating the folder
            response = WebDavMethod.Unlock(uri.ToString());
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.NoContent, (int)httpWebResponse.StatusCode);
        }
        #endregion

        #region PROPPATCH

        /// <summary>
        /// Try to make a WebDAV Put request.
        /// Expect the statuscode 201 Created.
        /// </summary>
        [TestMethod]
        public void Proppatch_ValidRequest_TestMethod()
        {
            // create uri to non-existent file or folder
            var uri = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, delete it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode == (int)HttpStatusCode.NotFound)
            {
                WebDavMethod.Put(uri.ToString());
            }

            // get the response for creating the folder
            const string xmlBody = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                                   "<D:propertyupdate xmlns:D=\"DAV:\" xmlns:Z=\"urn:schemas-microsoft-com:\">" +
                                   "<D:set><D:prop><Z:Win32CreationTime>Mon, 05 May 2014 08:10:08 GMT" +
                                   "</Z:Win32CreationTime><Z:Win32LastAccessTime>Mon, 05 May 2014 08:10:08 GMT" +
                                   "</Z:Win32LastAccessTime><Z:Win32LastModifiedTime>Mon, 05 May 2014 08:10:08 GMT" +
                                   "</Z:Win32LastModifiedTime><Z:Win32FileAttributes>00000020" +
                                   "</Z:Win32FileAttributes></D:prop></D:set></D:propertyupdate>";
            response = WebDavMethod.Proppatch(uri.ToString(), xmlBody);
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)WebDavStatusCode.MultiStatus, (int)httpWebResponse.StatusCode);
        }

        #endregion

        #region GET

        /// <summary>
        /// Try to make a WebDAV Get request.
        /// Expect the statuscode 200 Ok.
        /// </summary>
        [TestMethod]
        public void Get_ValidRequest_TestMethod()
        {
            // create uri
            var uri = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, create it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode != (int)WebDavStatusCode.MultiStatus)
            {
                WebDavMethod.Put(uri.ToString());
            }

            // get the response for deleting the folder
            response = WebDavMethod.Get(uri.ToString());
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.OK, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV Get request.
        /// Expect the statuscode 404 Not Found.
        /// </summary>
        [TestMethod]
        public void Get_UnvalidRequest_NotFound_TestMethod()
        {
            // create uri
            var uri = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());

            // search for the file or folder
            var response = WebDavMethod.Propfind(uri.ToString(), "0");
            var httpWebResponse = response as HttpWebResponse;

            // if there is a file or folder at the URI, create it
            if (httpWebResponse != null && (int)httpWebResponse.StatusCode != (int)HttpStatusCode.NotFound)
            {
                WebDavMethod.Delete(uri.ToString());
            }

            // get the response for deleting the folder
            response = WebDavMethod.Get(uri.ToString());
            httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.NotFound, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Try to make a WebDAV Get request.
        /// Expect the statuscode 409 Conflict.
        /// </summary>
        [TestMethod]
        public void Get_UnvalidRequest_Conflict_TestMethod()
        {
            // create uri
            var path = Path.GetRandomFileName() + "/" + Path.GetRandomFileName();
            var uri = new Uri(WebDavConfig.WebDavTestBaseUri, path);

            // get the response for deleting the folder
            var response = WebDavMethod.Get(uri.ToString());
            var httpWebResponse = response as HttpWebResponse;

            // if response is null, fail the _test
            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // else check the result
            Assert.AreEqual((int)HttpStatusCode.Conflict, (int)httpWebResponse.StatusCode);
        }

        #endregion

    }
}
