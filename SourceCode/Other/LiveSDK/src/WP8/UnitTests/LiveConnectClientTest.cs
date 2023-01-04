// ------------------------------------------------------------------------------
// Copyright 2014 Microsoft Corporation
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ------------------------------------------------------------------------------

namespace Microsoft.Live.WP8.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LiveConnectClientTest
    {

        #region Constructor tests

        [TestMethod]
        public void TestConstructorNullLiveConnectSession()
        {
            try
            {
                new LiveConnectClient(null);
                Assert.Fail("Expected ArgumentNullException to be thrown.");
            }
            catch (ArgumentNullException)
            {
            }
        }

        #endregion

        #region Get tests

        [TestMethod]
        public void TestGetNullPath()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.GetAsync(null);
                Assert.Fail("Expected ArguementNullException to be thrown.");
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void TestGetEmptyStringPath()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.GetAsync(string.Empty);
                Assert.Fail("Expected ArguementException to be thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void TestGetWhiteSpaceStringPath()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.GetAsync("\t\n ");
                Assert.Fail("Expected ArguementException to be thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        #endregion

        #region Delete tests

        [TestMethod]
        public void TestDeleteNullPath()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.DeleteAsync(null);
                Assert.Fail("Expected ArguementNullException to be thrown.");
            }
            catch (ArgumentNullException)
            {
            }
        }

        public void TestDeleteEmptyStringPath()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.DeleteAsync(string.Empty);
                Assert.Fail("Expected ArguementException to be thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void TestDeleteWhiteSpaceStringPath()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.DeleteAsync("\t\n ");
                Assert.Fail("Expected ArguementException to be thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        #endregion

        #region Put tests

        [TestMethod]
        public void TestPutNullPath()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.PutAsync(null, new Dictionary<string, object>());
                Assert.Fail("Expected ArguementNullException to be thrown.");
            }
            catch (ArgumentNullException)
            {
            }
        }

        public void TestPutEmptyStringPath()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.PutAsync(string.Empty, new Dictionary<string, object>());
                Assert.Fail("Expected ArguementException to be thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void TestPutWhiteSpaceStringPath()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.PutAsync("\t\n ", new Dictionary<string, object>());
                Assert.Fail("Expected ArguementException to be thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void TestPutNullDictionaryBody()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                IDictionary<string, object> body = null;
                connectClient.PutAsync("fileId.123", body);
                Assert.Fail("Expected ArguementNullException to be thrown.");
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void TestPutNullStringBody()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                string body = null;
                connectClient.PutAsync("fileId.123", body);
                Assert.Fail("Expected ArguementNullException to be thrown.");
            }
            catch (ArgumentNullException)
            {
            }
        }

        public void TestPutEmptyStringBody()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.PutAsync("fileId.123", string.Empty);
                Assert.Fail("Expected ArguementException to be thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void TestPutWhiteSpaceStringBody()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.PutAsync("fileId.123", "\t\n ");
                Assert.Fail("Expected ArguementException to be thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        #endregion

        #region Post tests

        [TestMethod]
        public void TestPostNullPath()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.PostAsync(null, new Dictionary<string, object>());
                Assert.Fail("Expected ArguementNullException to be thrown.");
            }
            catch (ArgumentNullException)
            {
            }
        }

        public void TestPostEmptyStringPath()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.PostAsync(string.Empty, new Dictionary<string, object>());
                Assert.Fail("Expected ArguementException to be thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void TestPostWhiteSpaceStringPath()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.PostAsync("\t\n ", new Dictionary<string, object>());
                Assert.Fail("Expected ArguementException to be thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void TestPostNullDictionaryBody()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                IDictionary<string, object> body = null;
                connectClient.PostAsync("fileId.123", body);
                Assert.Fail("Expected ArguementNullException to be thrown.");
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void TestPostNullStringBody()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                string body = null;
                connectClient.PostAsync("fileId.123", body);
                Assert.Fail("Expected ArguementNullException to be thrown.");
            }
            catch (ArgumentNullException)
            {
            }
        }

        public void TestPostEmptyStringBody()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.PostAsync("fileId.123", string.Empty);
                Assert.Fail("Expected ArguementException to be thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void TestPostWhiteSpaceStringBody()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.PutAsync("fileId.123", "\t\n ");
                Assert.Fail("Expected ArguementException to be thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        #endregion

        #region Copy tests

        [TestMethod]
        public void TestCopyNullPath()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.CopyAsync(null, "fileId.123");
                Assert.Fail("Expected ArguementNullException to be thrown.");
            }
            catch (ArgumentNullException)
            {
            }
        }

        public void TestCopyEmptyStringPath()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.CopyAsync(string.Empty, "fileId.123");
                Assert.Fail("Expected ArguementException to be thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void TestCopyWhiteSpaceStringPath()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.CopyAsync("\t\n ", "fileId.123");
                Assert.Fail("Expected ArguementException to be thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void TestCopyNullDestination()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.CopyAsync("fileId.123", null);
                Assert.Fail("Expected ArguementNullException to be thrown.");
            }
            catch (ArgumentNullException)
            {
            }
        }

        public void TestCopyEmptyStringDestination()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.CopyAsync("fileId.123", string.Empty);
                Assert.Fail("Expected ArguementException to be thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void TestCopyWhiteSpaceStringDestination()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.CopyAsync("fileId.123", "\t\n ");
                Assert.Fail("Expected ArguementException to be thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        #endregion

        #region Move tests

        [TestMethod]
        public void TestMoveNullPath()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.MoveAsync(null, "fileId.123");
                Assert.Fail("Expected ArguementNullException to be thrown.");
            }
            catch (ArgumentNullException)
            {
            }
        }

        public void TestMoveEmptyStringPath()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.MoveAsync(string.Empty, "fileId.123");
                Assert.Fail("Expected ArguementException to be thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void TestMoveWhiteSpaceStringPath()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.MoveAsync("\t\n ", "fileId.123");
                Assert.Fail("Expected ArguementException to be thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void TestMoveNullDestination()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.MoveAsync("fileId.123", null);
                Assert.Fail("Expected ArguementNullException to be thrown.");
            }
            catch (ArgumentNullException)
            {
            }
        }

        public void TestMoveEmptyStringDestination()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.MoveAsync("fileId.123", string.Empty);
                Assert.Fail("Expected ArguementException to be thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void TestMoveWhiteSpaceStringDestination()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                connectClient.MoveAsync("fileId.123", "\t\n ");
                Assert.Fail("Expected ArguementException to be thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        #endregion

        #region BackgroundDownload tests

        [TestMethod]
        public async void TestBackgroundDownloadNullPath()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                var downloadLocation = new Uri("/shared/transfers/downloadLocation.txt", UriKind.RelativeOrAbsolute);
                await connectClient.BackgroundDownloadAsync(null, downloadLocation);
                Assert.Fail("Expected ArguementException to be thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        #endregion

        #region GetPendingDownload tests

        // TODO

        #endregion

        #region BackgroundUpload tests

        // TODO

        #endregion

        #region GetPendingUpload tests

        [TestMethod]
        public void TestBackgroundUploadNullPath()
        {
            var connectClient = new LiveConnectClient(new LiveConnectSession());
            try
            {
                var uploadLocation = new Uri("/shared/transfers/uploadLocation.txt", UriKind.RelativeOrAbsolute);
                connectClient.BackgroundUploadAsync(
                    null,
                    uploadLocation,
                    OverwriteOption.Overwrite);
                Assert.Fail("Expected ArguementException to be thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        #endregion

        #region Download tests

        // TODO

        #endregion

        #region Upload tests

        // TODO

        #endregion
    }
}
