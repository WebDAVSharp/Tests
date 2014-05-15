using System;
using System.IO;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebDAVSharp.Server;
using WebDAVSharp.Server.Utilities;

namespace WebDAVTests
{
    /// <summary>
    /// Tests to check the time it takes for making and deleting 100 collections
    /// and for putting and deleting 1000 files
    /// </summary>
    [TestClass]
    public class UnitTestSpeed
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
        /// Test where 100 folders are created and deleted afterwards.
        /// Methods: Propfind, MkCol, Delete.
        /// The time needed for this test is show in the Test Explorer.
        /// </summary>
        [TestMethod]
        public void SpeedTest_MkColAndDelete100Folders()
        {
            HttpWebResponse httpWebResponse = null;

            for (int i = 0; i < 100; i++)
            {
                // create uri to file or folder
                var uri = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());

                // search for the file or folder
                var response = WebDavMethod.Propfind(uri.ToString(), "0");
                httpWebResponse = response as HttpWebResponse;

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
                else if ((int)HttpStatusCode.OK != (int)httpWebResponse.StatusCode)
                {
                    break;
                }
            }

            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // check the result
            Assert.AreEqual((int)HttpStatusCode.OK, (int)httpWebResponse.StatusCode);
        }

        /// <summary>
        /// Test where 1000 files are created and deleted afterwards.
        /// Methods: Propfind, Put, Delete.
        /// The time needed for this test is show in the Test Explorer.
        /// </summary>
        [TestMethod]
        public void SpeedTest_PutAndDelete1000Files()
        {
            HttpWebResponse httpWebResponse = null;

            for (int i = 0; i < 1000; i++)
            {
                // create uri to file or folder
                var uri = new Uri(WebDavConfig.WebDavTestBaseUri, Path.GetRandomFileName());

                // search for the file or folder
                var response = WebDavMethod.Propfind(uri.ToString(), "0");
                httpWebResponse = response as HttpWebResponse;

                // if there is a file or folder at the URI, create it
                if (httpWebResponse != null && (int)httpWebResponse.StatusCode != (int)WebDavStatusCode.MultiStatus)
                {
                    WebDavMethod.Put(uri.ToString());
                }

                // get the response for deleting the folder
                response = WebDavMethod.Delete(uri.ToString());
                httpWebResponse = response as HttpWebResponse;

                // if response is null, fail the _test
                if (httpWebResponse == null)
                {
                    Assert.Fail("The HttpWebResponse equals null.");
                }
                else if ((int)HttpStatusCode.OK != (int)httpWebResponse.StatusCode)
                {
                    break;
                }
            }

            if (httpWebResponse == null)
            {
                Assert.Fail("The HttpWebResponse equals null.");
            }
            // check the result
            Assert.AreEqual((int)HttpStatusCode.OK, (int)httpWebResponse.StatusCode);
        }
    }
}
