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

namespace Microsoft.Live.Phone.Operations
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.Live.Operations;
    using Microsoft.Phone.BackgroundTransfer;

    /// <summary>
    /// Class that first gets the upload_location from the api service.
    /// Then builds a BackgroundTransferRequest for uploads and gives it to the BackgroundTransferService.
    /// Then converts any events on the BackgroundTransferRequest.
    /// </summary>
    internal class BackgroundUploadOperation
    {
        #region Nested types

        /// <summary>
        /// Class that builds the BackgroundUploadOperation because of its long list of parameters.
        /// </summary>
        public class Builder
        {
            private string path;
            private Uri uploadLocationOnDevice;
            private LiveConnectClient client;
            private IBackgroundTransferService backgroundTransferService;

            public Builder()
            {
                this.backgroundTransferService = PhoneBackgroundTransferService.Instance;
            }

            public string Path
            {
                get
                {
                    return this.path;
                }
                set
                {
                    Debug.Assert(value != null);
                    this.path = value;
                }
            }

            public Uri UploadLocationOnDevice
            {
                get
                {
                    return this.uploadLocationOnDevice;
                }

                set
                {
                    Debug.Assert(value != null);
                    this.uploadLocationOnDevice = value;
                }
            }

            public LiveConnectClient Client
            {
                get
                {
                    return this.client;
                }
                set
                {
                    Debug.Assert(value != null);
                    this.client = value;
                }
            }

            public IBackgroundTransferService BackgroundTransferService
            {
                get
                {
                    return this.backgroundTransferService;
                }
                set
                {
                    Debug.Assert(value != null);
                    this.backgroundTransferService = value;
                }
            }

            public OverwriteOption OverwriteOption { get; set; }

            public IProgress<LiveOperationProgress> Progress { get; set; }

            public BackgroundTransferPreferences BackgroundTransferPreferences { get; set; }

            public BackgroundUploadOperation Build()
            {
                Debug.Assert(this.path != null);
                Debug.Assert(this.uploadLocationOnDevice != null);
                Debug.Assert(this.client != null);
                Debug.Assert(this.backgroundTransferService != null);

                return new BackgroundUploadOperation(this);
            }
        }

        #endregion

        #region Fields

        private readonly string path;
        private readonly Uri uploadLocationOnDevice;
        private readonly LiveConnectClient client;
        private readonly IBackgroundTransferService backgroundTransferService;
        private readonly OverwriteOption overwriteOption;
        private readonly IProgress<LiveOperationProgress> progress;
        private readonly TaskCompletionSource<LiveOperationResult> tcs;
        private readonly TransferPreferences transferPreferences;

        private GetUploadLinkOperation getUploadLinkOperation;
        private BackgroundTransferRequest request;
        private bool requestAddedToService;
        private OperationStatus status;

        #endregion

        #region Constructors

        private BackgroundUploadOperation(Builder builder)
        {
            this.path = builder.Path;
            this.uploadLocationOnDevice = builder.UploadLocationOnDevice;
            this.client = builder.Client;
            this.backgroundTransferService = builder.BackgroundTransferService;
            this.overwriteOption = builder.OverwriteOption;
            this.progress = builder.Progress;
            this.tcs = new TaskCompletionSource<LiveOperationResult>();
            this.requestAddedToService = false;
            this.status = OperationStatus.NotStarted;
            this.transferPreferences =
                BackgroundTransferHelper.GetTransferPreferences(builder.BackgroundTransferPreferences);
        }

        #endregion

        #region Public Methods

        public void Cancel()
        {
            // if it is already cancelled or completed then there is nothing to cancel.
            if (this.status == OperationStatus.Cancelled || this.status == OperationStatus.Completed)
            {
                return;
            }

            // the getUploadLinkOperation could of not been created yet (i.e., Cancel is called before ExecuteAsync).
            if (this.getUploadLinkOperation != null)
            {
                this.getUploadLinkOperation.Cancel();
            }

            if (this.requestAddedToService)
            {
                this.backgroundTransferService.Remove(this.request);
            }

            this.status = OperationStatus.Cancelled;
            this.tcs.TrySetCanceled();
        }

        /// <summary>
        /// Performs the BackgroundUploadOperation.
        /// </summary>
        public async Task<LiveOperationResult> ExecuteAsync()
        {
            Debug.Assert(this.status != OperationStatus.Completed, "Cannot execute on a completed operation.");

            if (this.status == OperationStatus.Cancelled)
            {
                return await this.tcs.Task;
            }

            this.status = OperationStatus.Started;

            string filename = Path.GetFileName(this.uploadLocationOnDevice.OriginalString);
            Debug.Assert(!string.IsNullOrEmpty(filename));

            Uri requestUri = this.client.GetResourceUri(this.path, ApiMethod.Upload);
            // TODO: Figure out how to mock out GetUploadLinkOperation elegantly so this can be tested.
            this.getUploadLinkOperation = new GetUploadLinkOperation(
                this.client,
                requestUri,
                filename,
                this.overwriteOption,
                null);

            LiveOperationResult uploadLinkResult = await this.getUploadLinkOperation.ExecuteAsync();

            var uploadRequestUri = new Uri(uploadLinkResult.RawResult, UriKind.Absolute);
            var downloadLocationOnDevice = 
                new Uri(this.uploadLocationOnDevice.OriginalString + ".json", UriKind.RelativeOrAbsolute);
            var builder = new BackgroundUploadRequestBuilder
            {
                RequestUri = uploadRequestUri,
                AccessToken = (this.client.Session != null) ? this.client.Session.AccessToken : "",
                UploadLocationOnDevice = this.uploadLocationOnDevice,
                DownloadLocationOnDevice = downloadLocationOnDevice,
                TransferPreferences = this.transferPreferences
            };

            this.request = builder.Build();

            var eventAdapter = new BackgroundUploadEventAdapter(this.backgroundTransferService, this.tcs);
            Task<LiveOperationResult> task = progress == null ? 
                                             eventAdapter.ConvertTransferStatusChangedToTask(this.request) :
                                             eventAdapter.ConvertTransferStatusChangedToTask(this.request, progress);

            Debug.Assert(this.tcs.Task == task, "EventAdapter returned a different task. This could affect cancel.");

            if (this.status != OperationStatus.Cancelled)
            {
                this.backgroundTransferService.Add(this.request);
                this.requestAddedToService = true;
            }

            LiveOperationResult result = await task;
            this.status = OperationStatus.Completed;
            return result;
        }

        #endregion
    }
}
