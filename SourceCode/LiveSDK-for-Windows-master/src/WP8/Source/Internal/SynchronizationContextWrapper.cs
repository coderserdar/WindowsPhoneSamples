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

namespace Microsoft.Live
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    internal class SynchronizationContextWrapper
    {
        private static SynchronizationContextWrapper syncContxtWrapper;
        private readonly SynchronizationContext syncContext;

        public SynchronizationContextWrapper(SynchronizationContext syncContext)
        {
            this.syncContext = syncContext;
        }

        static public SynchronizationContextWrapper Current
        {
            get
            {
                SynchronizationContextWrapper.syncContxtWrapper = new SynchronizationContextWrapper(SynchronizationContext.Current);

                return SynchronizationContextWrapper.syncContxtWrapper;
            }
        }

        public void Post(Action callback)
        {
            Debug.Assert(callback != null);

            if (this.syncContext != null)
            {
                this.syncContext.Post(
                    delegate(object state)
                    {
                        callback();
                    }, null);
            }
            else
            {
                callback();
            }
        }
    }
}
